using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Usuarios
{
    public interface IUsuarioFlujo
    {
    //    Task<ResultadoPaginado<UsuarioDto>> ListarAsync(UsuarioFiltro filtro, CancellationToken ct);
        Task<UsuarioDto> ObtenerAsync(int id, CancellationToken ct);
        Task<UsuarioDto> CrearAsync(UsuarioCrearDtos dto, CancellationToken ct);
        Task ActualizarAsync(int id, UsuarioDtos dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);
    }
}
