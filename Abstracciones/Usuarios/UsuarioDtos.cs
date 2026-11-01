using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Usuarios
{
    public class UsuarioCrearDtos
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos.")]
        public string Telefono { get; set; } = string.Empty;
    }

    public class UsuarioDtos : UsuarioCrearDtos { }

    /// <summary>Salida hacia el cliente HTTP. Nunca se expone la entidad directamente.</summary>
    public record UsuarioDto(
        int Id,
        string Nombre,
        string Correo,
        string Telefono,
        DateTime FechaRegistro);

    public class ClienteFiltro : ParametrosPaginacion
    {
        /// <summary>Texto buscado en nombre, correo.</summary>
        [StringLength(100)]
        public string? Q { get; set; }
    }
}
