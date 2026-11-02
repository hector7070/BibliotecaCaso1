using Abstracciones.PrestamoLibros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.PrestamoLibros
{
    public class PrestamoLibroConfiguracion : IEntityTypeConfiguration<PrestamoLibro>
    {
        public void Configure(EntityTypeBuilder<PrestamoLibro> b)
        {
            b.ToTable("PrestamosLibros");
            b.HasKey(p => p.Id);

            b.Property(p => p.Observaciones).HasMaxLength(300);

            b.Property(p => p.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();

            b.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(p => p.Libro)
                .WithMany()
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(p => new { p.LibroId, p.Estado });
            b.HasIndex(p => p.UsuarioId);
            b.HasIndex(p => p.FechaPrestamo);
        }
    }
}
