using Abstracciones.Libros;
using Abstracciones.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace DA.Contexto
{
    public class BibliotecaContexto : DbContext
    {
        public BibliotecaContexto(DbContextOptions<BibliotecaContexto> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Libro> Libros => Set<Libro>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaContexto).Assembly);
        }
    }
}
