using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.PrestamoLibros;
using DA.Comunes;
using DA.Contexto;
using Microsoft.EntityFrameworkCore;

namespace DA.PrestamoLibros
{
    public class PrestamoLibroDA : IPrestamoLibroDA
    {
        private readonly BibliotecaContexto _ctx;
        private readonly TimeProvider _reloj;

        public PrestamoLibroDA(BibliotecaContexto ctx, TimeProvider reloj)
        {
            _ctx = ctx;
            _reloj = reloj;
        }

        public async Task<ResultadoPaginado<PrestamoLibro>> BuscarAsync(PrestamoLibroFiltro filtro, CancellationToken ct)
        {
            IQueryable<PrestamoLibro> consulta = _ctx.PrestamosLibros
                .AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Libro);

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(p =>
                    (p.Usuario != null && p.Usuario.Nombre.Contains(texto)) ||
                    (p.Libro != null && p.Libro.Titulo.Contains(texto)) ||
                    (p.Libro != null && p.Libro.ISBN.Contains(texto)));
            }

            if (filtro.UsuarioId.HasValue)
                consulta = consulta.Where(p => p.UsuarioId == filtro.UsuarioId.Value);

            if (filtro.LibroId.HasValue)
                consulta = consulta.Where(p => p.LibroId == filtro.LibroId.Value);

            if (filtro.Estado.HasValue)
                consulta = consulta.Where(p => p.Estado == filtro.Estado.Value);

            if (filtro.Vencidos == true)
            {
                var ahora = _reloj.GetUtcNow().UtcDateTime;
                consulta = consulta.Where(p => p.Estado == EstadoPrestamo.Activo && p.FechaVencimiento < ahora);
            }
            else if (filtro.Vencidos == false)
            {
                var ahora = _reloj.GetUtcNow().UtcDateTime;
                consulta = consulta.Where(p => p.Estado == EstadoPrestamo.Devuelto || p.FechaVencimiento >= ahora);
            }

            var total = await consulta.CountAsync(ct);

            var items = await consulta
                .OrderByDescending(p => p.FechaPrestamo).ThenBy(p => p.Id)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<PrestamoLibro>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public Task<PrestamoLibro?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.PrestamosLibros.AsNoTracking().Include(p => p.Usuario).Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

        public Task<bool> TienePrestamoActivoAsync(int libroId, CancellationToken ct) =>
            _ctx.PrestamosLibros.AnyAsync(p => p.LibroId == libroId && p.Estado == EstadoPrestamo.Activo, ct);

        public async Task<PrestamoLibro> AgregarAsync(PrestamoLibro prestamo, CancellationToken ct)
        {
            _ctx.PrestamosLibros.Add(prestamo);
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("El usuario o el libro indicado no existe.");
            }
            return prestamo;
        }

        public async Task<bool> RegistrarDevolucionAsync(int id, DateTime fecha, CancellationToken ct)
        {
            var existente = await _ctx.PrestamosLibros.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (existente is null) return false;

            existente.FechaDevolucion = fecha;
            existente.Estado = EstadoPrestamo.Devuelto;
            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _ctx.PrestamosLibros.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (existente is null) return false;

            _ctx.PrestamosLibros.Remove(existente);
            await _ctx.SaveChangesAsync(ct);
            return true;
        }
    }
}
