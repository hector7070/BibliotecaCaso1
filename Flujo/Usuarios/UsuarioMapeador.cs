using Abstracciones.Usuarios;

namespace Flujo.Usuarios
{
    public static class UsuarioMapeador
    {
        public static UsuarioDto ADto(Usuario u) =>
            new(u.Id, u.Nombre, u.Correo, u.Telefono, u.FechaRegistro);
    }
}
