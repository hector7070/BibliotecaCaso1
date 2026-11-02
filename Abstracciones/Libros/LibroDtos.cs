using System.ComponentModel.DataAnnotations;
using Abstracciones.Comunes;

namespace Abstracciones.Libros
{
    public class LibroCrearDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El autor debe tener entre 2 y 100 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(50)]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La editorial es obligatoria.")]
        [StringLength(100)]
        public string Editorial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;
    }

    public class LibroActualizarDto : LibroCrearDto
    {
        public bool Activo { get; set; } = true;
    }

    public record LibroDto(
        int Id,
        string Titulo,
        string Autor,
        string Genero,
        string Editorial,
        string ISBN,
        bool Activo);

    public class LibroFiltro : ParametrosPaginacion
    {
        [StringLength(100)]
        public string? Q { get; set; }
    }
}
