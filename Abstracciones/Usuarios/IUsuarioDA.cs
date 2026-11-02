using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Usuarios
{
    public interface IUsuarioDA
    {
    //    Task<ResultadoPaginado<Usuario>> BuscarAsync(UsuarioFiltro filtro, CancellationToken ct);
        Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken ct);
        Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken ct);

        /// <summary>Actualiza Nombre, Correo, Telefono. Devuelve false si el usuario no existe.</summary>
        Task<bool> ActualizarAsync(Usuario datos, CancellationToken ct);

        Task<bool> EliminarAsync(int id, CancellationToken ct);
        Task<bool> TieneLibrosAsync(int usuarioId, CancellationToken ct);

    }
}
