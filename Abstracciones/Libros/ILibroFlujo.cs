using Abstracciones.Comunes;

namespace Abstracciones.Libros
{
    public interface ILibroFlujo
    {
        Task<ResultadoPaginado<LibroDto>> ListarAsync(LibroFiltro filtro, CancellationToken ct);
        Task<LibroDto> ObtenerAsync(int id, CancellationToken ct);
        Task<LibroDto> CrearAsync(LibroCrearDto dto, CancellationToken ct);
        Task ActualizarAsync(int id, LibroActualizarDto dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);
    }
}
