using Abstracciones.Comunes;

namespace Abstracciones.Usuarios
{
    public interface IUsuarioFlujo
    {
        Task<ResultadoPaginado<UsuarioDto>> ListarAsync(UsuarioFiltro filtro, CancellationToken ct);
        Task<UsuarioDto> ObtenerAsync(int id, CancellationToken ct);
        Task<UsuarioDto> CrearAsync(UsuarioCrearDto dto, CancellationToken ct);
        Task ActualizarAsync(int id, UsuarioActualizarDto dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);
    }
}
