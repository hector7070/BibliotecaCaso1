using Abstracciones.PrestamoLibros;

namespace Flujo.PrestamoLibros
{
    public static class PrestamoLibroMapeador
    {
        public static PrestamoLibroDto ADto(PrestamoLibro p, DateTime ahora) =>
            new(
                p.Id,
                p.UsuarioId,
                p.Usuario?.Nombre ?? string.Empty,
                p.LibroId,
                p.Libro?.Titulo ?? string.Empty,
                p.Libro?.ISBN ?? string.Empty,
                p.FechaPrestamo,
                p.FechaVencimiento,
                p.FechaDevolucion,
                p.Estado,
                p.Estado == EstadoPrestamo.Activo && p.FechaVencimiento < ahora,
                p.Observaciones);
    }
}
