using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Libros;
using Abstracciones.PrestamoLibros;
using Abstracciones.Usuarios;

namespace Flujo.PrestamoLibros
{
    public class PrestamoLibroFlujo : IPrestamoLibroFlujo
    {
        private const int DiasPrestamoPorDefecto = 7;

        private readonly IPrestamoLibroDA _da;
        private readonly IUsuarioDA _usuarioDA;
        private readonly ILibroDA _libroDA;
        private readonly TimeProvider _reloj;

        public PrestamoLibroFlujo(IPrestamoLibroDA da, IUsuarioDA usuarioDA, ILibroDA libroDA, TimeProvider reloj)
        {
            _da = da;
            _usuarioDA = usuarioDA;
            _libroDA = libroDA;
            _reloj = reloj;
        }

        public async Task<ResultadoPaginado<PrestamoLibroDto>> ListarAsync(PrestamoLibroFiltro filtro, CancellationToken ct)
        {
            var pagina = await _da.BuscarAsync(filtro, ct);
            var ahora = _reloj.GetUtcNow().UtcDateTime;
            return new ResultadoPaginado<PrestamoLibroDto>
            {
                Items = pagina.Items.Select(p => PrestamoLibroMapeador.ADto(p, ahora)).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<PrestamoLibroDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var prestamo = await _da.ObtenerPorIdAsync(id, ct)
                ?? throw new NoEncontradoException("Préstamo", id);
            return PrestamoLibroMapeador.ADto(prestamo, _reloj.GetUtcNow().UtcDateTime);
        }

        public async Task<PrestamoLibroDto> CrearAsync(PrestamoLibroCrearDto dto, CancellationToken ct)
        {
            if (await _usuarioDA.ObtenerPorIdAsync(dto.UsuarioId, ct) is null)
                throw new ValidacionNegocioException($"El usuario {dto.UsuarioId} no existe.");

            var libro = await _libroDA.ObtenerPorIdAsync(dto.LibroId, ct)
                ?? throw new ValidacionNegocioException($"El libro {dto.LibroId} no existe.");

            if (!libro.Activo)
                throw new ConflictoException($"El libro '{libro.Titulo}' no está disponible para préstamo.");

            if (await _da.TienePrestamoActivoAsync(dto.LibroId, ct))
                throw new ConflictoException($"El libro '{libro.Titulo}' ya tiene un préstamo activo.");

            var ahora = _reloj.GetUtcNow().UtcDateTime;
            var dias = dto.DiasPrestamo > 0 ? dto.DiasPrestamo : DiasPrestamoPorDefecto;

            var prestamo = new PrestamoLibro
            {
                UsuarioId = dto.UsuarioId,
                LibroId = dto.LibroId,
                FechaPrestamo = ahora,
                FechaVencimiento = ahora.AddDays(dias),
                Estado = EstadoPrestamo.Activo,
                Observaciones = LimpiarOpcional(dto.Observaciones)
            };

            await _da.AgregarAsync(prestamo, ct);
            return await ObtenerAsync(prestamo.Id, ct);
        }

        public async Task<PrestamoLibroDto> RegistrarDevolucionAsync(int id, CancellationToken ct)
        {
            var prestamo = await _da.ObtenerPorIdAsync(id, ct)
                ?? throw new NoEncontradoException("Préstamo", id);

            if (prestamo.Estado == EstadoPrestamo.Devuelto)
                throw new ConflictoException("El préstamo ya fue devuelto.");

            var fecha = _reloj.GetUtcNow().UtcDateTime;
            if (!await _da.RegistrarDevolucionAsync(id, fecha, ct))
                throw new NoEncontradoException("Préstamo", id);

            return await ObtenerAsync(id, ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            var prestamo = await _da.ObtenerPorIdAsync(id, ct)
                ?? throw new NoEncontradoException("Préstamo", id);

            if (prestamo.Estado == EstadoPrestamo.Activo)
                throw new ConflictoException("No se puede eliminar un préstamo activo; primero registre la devolución.");

            if (!await _da.EliminarAsync(id, ct))
                throw new NoEncontradoException("Préstamo", id);
        }

        private static string? LimpiarOpcional(string? texto) =>
            string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
    }
}
