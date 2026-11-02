using Abstracciones.Libros;
using Abstracciones.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.Libros
{
    public class LibroMapeador
    {

        public static LibroDto ADto(Libro l) =>
        new(l.Id, l.Titulo, l.Autor,  l.Genero, l.Editorial, l.ISBN, l.Activo);

    }
}
