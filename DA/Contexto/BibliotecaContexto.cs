using Abstracciones.Libros;
using Abstracciones.PrestamoLibros;
using Abstracciones.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace DA.Contexto
{
    public class BibliotecaContexto : DbContext
    {
        public BibliotecaContexto(DbContextOptions<BibliotecaContexto> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Libro> Libros => Set<Libro>();
        public DbSet<PrestamoLibro> PrestamosLibros => Set<PrestamoLibro>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaContexto).Assembly);
        }
    }
}
