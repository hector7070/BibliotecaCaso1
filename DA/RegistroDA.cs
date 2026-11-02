using Abstracciones.Libros;
using Abstracciones.Usuarios;
using DA.Contexto;
using DA.Libros;
using DA.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DA
{
    public static class RegistroDA
    {
        public static IServiceCollection AddDatos(this IServiceCollection servicios, string cadenaConexion)
        {
            servicios.AddDbContext<BibliotecaContexto>(o => o.UseSqlServer(cadenaConexion));
            servicios.AddScoped<IUsuarioDA, UsuarioDA>();
            servicios.AddScoped<ILibroDA, LibroDA>();
            return servicios;
        }
    }
}
