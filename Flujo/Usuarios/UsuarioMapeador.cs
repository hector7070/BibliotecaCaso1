using Abstracciones.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.Usuarios
{
    public class UsuarioMapeador
    {
        public static UsuarioDto ADto(Usuario u) =>
      new(u.Id, u.Nombre, u.Correo, u.Telefono, u.FechaRegistro);
    }
}
