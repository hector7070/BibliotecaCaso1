using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Libros;
using DA.Comunes;
using DA.Contexto;
using Microsoft.EntityFrameworkCore;

namespace DA.Libros
{
    public class LibroDA : ILibroDA
    {
        private readonly BibliotecaContexto _ctx;

        public LibroDA(BibliotecaContexto ctx) => _ctx = ctx;

        public async Task<ResultadoPaginado<Libro>> BuscarAsync(LibroFiltro filtro, CancellationToken ct)
        {
            IQueryable<Libro> consulta = _ctx.Libros.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(l =>
                    l.Titulo.Contains(texto) ||
                    l.Autor.Contains(texto) ||
                    l.ISBN.Contains(texto) ||
                    l.Genero.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            var items = await consulta
                .OrderBy(l => l.Titulo).ThenBy(l => l.Id)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<Libro>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public Task<Libro?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, ct);

        public Task<Libro?> ObtenerPorIsbnAsync(string isbn, CancellationToken ct) =>
            _ctx.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.ISBN == isbn, ct);

        public async Task<Libro> AgregarAsync(Libro libro, CancellationToken ct)
        {
            _ctx.Libros.Add(libro);
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe un libro con el ISBN '{libro.ISBN}'.");
            }
            return libro;
        }

        public async Task<bool> ActualizarAsync(Libro datos, CancellationToken ct)
        {
            var existente = await _ctx.Libros.FirstOrDefaultAsync(l => l.Id == datos.Id, ct);
            if (existente is null) return false;

            existente.Titulo = datos.Titulo;
            existente.Autor = datos.Autor;
            existente.Genero = datos.Genero;
            existente.Editorial = datos.Editorial;
            existente.ISBN = datos.ISBN;
            existente.Activo = datos.Activo;

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otro libro con el ISBN '{datos.ISBN}'.");
            }
            return true;
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _ctx.Libros.FirstOrDefaultAsync(l => l.Id == id, ct);
            if (existente is null) return false;

            _ctx.Libros.Remove(existente);
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("El libro tiene préstamos asociados y no se puede eliminar.");
            }
            return true;
        }
    }
}
