using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Comunes
{
    public class ParametrosPaginacion
    {
        public const int TamanoMaximo = 50;

        [Range(1, 100000, ErrorMessage = "La página debe estar entre 1 y 100000.")]
        public int Pagina { get; set; } = 1;

        [Range(1, TamanoMaximo, ErrorMessage = "El tamaño debe estar entre 1 y 50.")]
        public int Tamano { get; set; } = 10;
    }
}
