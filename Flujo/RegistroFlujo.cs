using Abstracciones.Libros;
using Abstracciones.Usuarios;
using Flujo.Libros;
using Flujo.Usuarios;
using Microsoft.Extensions.DependencyInjection;

namespace Flujo
{
    public static class RegistroFlujo
    {
        public static IServiceCollection AddFlujo(this IServiceCollection servicios)
        {
            servicios.AddSingleton(TimeProvider.System);
            servicios.AddScoped<IUsuarioFlujo, UsuarioFlujo>();
            servicios.AddScoped<ILibroFlujo, LibroFlujo>();
            return servicios;
        }
    }
}
