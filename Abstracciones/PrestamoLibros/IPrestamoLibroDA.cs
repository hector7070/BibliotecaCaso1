using Abstracciones.Comunes;

namespace Abstracciones.PrestamoLibros
{
    public interface IPrestamoLibroDA
    {
        Task<ResultadoPaginado<PrestamoLibro>> BuscarAsync(PrestamoLibroFiltro filtro, CancellationToken ct);
        Task<PrestamoLibro?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<bool> TienePrestamoActivoAsync(int libroId, CancellationToken ct);
        Task<PrestamoLibro> AgregarAsync(PrestamoLibro prestamo, CancellationToken ct);
        Task<bool> RegistrarDevolucionAsync(int id, DateTime fecha, CancellationToken ct);
        Task<bool> EliminarAsync(int id, CancellationToken ct);
    }
}
