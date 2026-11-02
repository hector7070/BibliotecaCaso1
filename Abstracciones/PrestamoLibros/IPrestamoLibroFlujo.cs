using Abstracciones.Comunes;

namespace Abstracciones.PrestamoLibros
{
    public interface IPrestamoLibroFlujo
    {
        Task<ResultadoPaginado<PrestamoLibroDto>> ListarAsync(PrestamoLibroFiltro filtro, CancellationToken ct);
        Task<PrestamoLibroDto> ObtenerAsync(int id, CancellationToken ct);
        Task<PrestamoLibroDto> CrearAsync(PrestamoLibroCrearDto dto, CancellationToken ct);
        Task<PrestamoLibroDto> RegistrarDevolucionAsync(int id, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);
    }
}
