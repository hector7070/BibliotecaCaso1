using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Usuarios;

namespace Flujo.Usuarios
{
    public class UsuarioFlujo : IUsuarioFlujo
    {
        private readonly IUsuarioDA _da;
        private readonly TimeProvider _reloj;

        public UsuarioFlujo(IUsuarioDA da, TimeProvider reloj)
        {
            _da = da;
            _reloj = reloj;
        }

        public async Task<ResultadoPaginado<UsuarioDto>> ListarAsync(UsuarioFiltro filtro, CancellationToken ct)
        {
            var pagina = await _da.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<UsuarioDto>
            {
                Items = pagina.Items.Select(UsuarioMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<UsuarioDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var usuario = await _da.ObtenerPorIdAsync(id, ct)
                ?? throw new NoEncontradoException("Usuario", id);
            return UsuarioMapeador.ADto(usuario);
        }

        public async Task<UsuarioDto> CrearAsync(UsuarioCrearDto dto, CancellationToken ct)
        {
            var correo = NormalizarCorreo(dto.Correo);

            if (await _da.ObtenerPorCorreoAsync(correo, ct) is not null)
                throw new ConflictoException($"Ya existe un usuario con el correo '{correo}'.");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Correo = correo,
                Telefono = dto.Telefono.Trim(),
                FechaRegistro = _reloj.GetUtcNow().UtcDateTime
            };

            await _da.AgregarAsync(usuario, ct);
            return UsuarioMapeador.ADto(usuario);
        }

        public async Task ActualizarAsync(int id, UsuarioActualizarDto dto, CancellationToken ct)
        {
            var correo = NormalizarCorreo(dto.Correo);

            _ = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Usuario", id);

            var otro = await _da.ObtenerPorCorreoAsync(correo, ct);
            if (otro is not null && otro.Id != id)
                throw new ConflictoException($"Ya existe otro usuario con el correo '{correo}'.");

            var actualizado = await _da.ActualizarAsync(new Usuario
            {
                Id = id,
                Nombre = dto.Nombre.Trim(),
                Correo = correo,
                Telefono = dto.Telefono.Trim()
            }, ct);

            if (!actualizado) throw new NoEncontradoException("Usuario", id);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Usuario", id);

            if (await _da.TieneLibrosAsync(id, ct))
                throw new ConflictoException("El usuario tiene libros asociados y no se puede eliminar.");

            if (!await _da.EliminarAsync(id, ct))
                throw new NoEncontradoException("Usuario", id);
        }

        private static string NormalizarCorreo(string correo) => correo.Trim().ToLowerInvariant();
    }
}
