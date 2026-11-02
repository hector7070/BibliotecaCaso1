using Abstracciones.Libros;

namespace Flujo.Libros
{
    public static class LibroMapeador
    {
        public static LibroDto ADto(Libro l) =>
            new(l.Id, l.Titulo, l.Autor, l.Genero, l.Editorial, l.ISBN, l.Activo);
    }
}
