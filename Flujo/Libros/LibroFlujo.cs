using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Libros;

namespace Flujo.Libros
{
    public class LibroFlujo : ILibroFlujo
    {
        private readonly ILibroDA _da;

        public LibroFlujo(ILibroDA da)
        {
            _da = da;
        }

        public async Task<ResultadoPaginado<LibroDto>> ListarAsync(LibroFiltro filtro, CancellationToken ct)
        {
            var pagina = await _da.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<LibroDto>
            {
                Items = pagina.Items.Select(LibroMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<LibroDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var libro = await _da.ObtenerPorIdAsync(id, ct)
                ?? throw new NoEncontradoException("Libro", id);
            return LibroMapeador.ADto(libro);
        }

        public async Task<LibroDto> CrearAsync(LibroCrearDto dto, CancellationToken ct)
        {
            var isbn = NormalizarIsbn(dto.ISBN);

            if (await _da.ObtenerPorIsbnAsync(isbn, ct) is not null)
                throw new ConflictoException($"Ya existe un libro con el ISBN '{isbn}'.");

            var libro = new Libro
            {
                Titulo = dto.Titulo.Trim(),
                Autor = dto.Autor.Trim(),
                Genero = dto.Genero.Trim(),
                Editorial = dto.Editorial.Trim(),
                ISBN = isbn,
                Activo = true
            };

            await _da.AgregarAsync(libro, ct);
            return LibroMapeador.ADto(libro);
        }

        public async Task ActualizarAsync(int id, LibroActualizarDto dto, CancellationToken ct)
        {
            var isbn = NormalizarIsbn(dto.ISBN);

            _ = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Libro", id);

            var otro = await _da.ObtenerPorIsbnAsync(isbn, ct);
            if (otro is not null && otro.Id != id)
                throw new ConflictoException($"Ya existe otro libro con el ISBN '{isbn}'.");

            var actualizado = await _da.ActualizarAsync(new Libro
            {
                Id = id,
                Titulo = dto.Titulo.Trim(),
                Autor = dto.Autor.Trim(),
                Genero = dto.Genero.Trim(),
                Editorial = dto.Editorial.Trim(),
                ISBN = isbn,
                Activo = dto.Activo
            }, ct);

            if (!actualizado) throw new NoEncontradoException("Libro", id);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Libro", id);

            if (!await _da.EliminarAsync(id, ct))
                throw new NoEncontradoException("Libro", id);
        }

        private static string NormalizarIsbn(string isbn) => isbn.Replace("-", "").Replace(" ", "").Trim().ToUpperInvariant();
    }
}
