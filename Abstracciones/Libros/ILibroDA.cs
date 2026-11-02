using Abstracciones.Comunes;

namespace Abstracciones.Libros
{
    public interface ILibroDA
    {
        Task<ResultadoPaginado<Libro>> BuscarAsync(LibroFiltro filtro, CancellationToken ct);
        Task<Libro?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<Libro?> ObtenerPorIsbnAsync(string isbn, CancellationToken ct);
        Task<Libro> AgregarAsync(Libro libro, CancellationToken ct);
        Task<bool> ActualizarAsync(Libro datos, CancellationToken ct);
        Task<bool> EliminarAsync(int id, CancellationToken ct);
    }
}
