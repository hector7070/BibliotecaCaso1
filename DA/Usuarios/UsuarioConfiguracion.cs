using Abstracciones.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Usuarios
{
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> b)
        {
            b.ToTable("Usuarios");
            b.HasKey(u => u.Id);

            b.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            b.Property(u => u.Correo).IsRequired().HasMaxLength(150);
            b.Property(u => u.Telefono).IsRequired().HasMaxLength(20);
            b.Property(u => u.FechaRegistro).HasDefaultValueSql("SYSUTCDATETIME()");

            b.HasIndex(u => u.Correo).IsUnique();
        }
    }
}
