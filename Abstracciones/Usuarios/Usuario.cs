using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Abstracciones.Usuarios
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        // Navegación 1-N: un usuario tiene muchos libros prestados.
       // public List<PrestamoLibro> PrestamoLibro { get; set; } = new();
    }
}
