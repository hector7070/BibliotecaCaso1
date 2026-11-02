using Abstracciones.Comunes;

namespace Abstracciones.Usuarios
{
    public interface IUsuarioDA
    {
        Task<ResultadoPaginado<Usuario>> BuscarAsync(UsuarioFiltro filtro, CancellationToken ct);
        Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken ct);
        Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken ct);
        Task<bool> ActualizarAsync(Usuario datos, CancellationToken ct);
        Task<bool> EliminarAsync(int id, CancellationToken ct);
        Task<bool> TieneLibrosAsync(int usuarioId, CancellationToken ct);
    }
}
