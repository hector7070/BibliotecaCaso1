using Abstracciones.Libros;
using Abstracciones.Usuarios;

namespace Abstracciones.PrestamoLibros
{
    public enum EstadoPrestamo
    {
        Activo,
        Devuelto
    }

    public class PrestamoLibro
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public EstadoPrestamo Estado { get; set; } = EstadoPrestamo.Activo;
        public string? Observaciones { get; set; }

        public Usuario? Usuario { get; set; }
        public Libro? Libro { get; set; }
    }
}
