using Abstracciones.Libros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Libros
{
    public class LibroConfiguracion : IEntityTypeConfiguration<Libro>
    {
        public void Configure(EntityTypeBuilder<Libro> b)
        {
            b.ToTable("Libros");
            b.HasKey(l => l.Id);

            b.Property(l => l.Titulo).IsRequired().HasMaxLength(150);
            b.Property(l => l.Autor).IsRequired().HasMaxLength(100);
            b.Property(l => l.Genero).IsRequired().HasMaxLength(50);
            b.Property(l => l.Editorial).IsRequired().HasMaxLength(100);
            b.Property(l => l.ISBN).IsRequired().HasMaxLength(20);
            b.Property(l => l.Activo).HasDefaultValue(true);

            b.HasIndex(l => l.ISBN).IsUnique();
        }
    }
}
