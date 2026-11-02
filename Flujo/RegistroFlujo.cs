using Abstracciones.Libros;
using Abstracciones.PrestamoLibros;
using Abstracciones.Usuarios;
using Flujo.Libros;
using Flujo.PrestamoLibros;
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
            servicios.AddScoped<IPrestamoLibroFlujo, PrestamoLibroFlujo>();
            return servicios;
        }
    }
}
