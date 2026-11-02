using System.ComponentModel.DataAnnotations;
using Abstracciones.Comunes;

namespace Abstracciones.PrestamoLibros
{
    public class PrestamoLibroCrearDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El libro es obligatorio.")]
        public int LibroId { get; set; }

        [Range(1, 90, ErrorMessage = "Los días de préstamo deben estar entre 1 y 90.")]
        public int DiasPrestamo { get; set; } = 7;

        [StringLength(300, ErrorMessage = "Las observaciones no pueden superar los 300 caracteres.")]
        public string? Observaciones { get; set; }
    }

    public record PrestamoLibroDto(
        int Id,
        int UsuarioId,
        string UsuarioNombre,
        int LibroId,
        string LibroTitulo,
        string LibroIsbn,
        DateTime FechaPrestamo,
        DateTime FechaVencimiento,
        DateTime? FechaDevolucion,
        EstadoPrestamo Estado,
        bool Vencido,
        string? Observaciones);

    public class PrestamoLibroFiltro : ParametrosPaginacion
    {
        [StringLength(100)]
        public string? Q { get; set; }

        [Range(1, int.MaxValue)]
        public int? UsuarioId { get; set; }

        [Range(1, int.MaxValue)]
        public int? LibroId { get; set; }

        public EstadoPrestamo? Estado { get; set; }

        public bool? Vencidos { get; set; }
    }
}
