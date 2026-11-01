using Abstracciones.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Usuarios
{
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> b)
        {
            b.ToTable("Usuarios");
            b.HasKey(u => u.Id);

            // Sin esto EF crea nvarchar(max) para todo: lento, sin índice posible
            // y sin protección. Los límites de aquí deben coincidir con las anotaciones de los DTOs.
            b.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            b.Property(u => u.Correo).IsRequired().HasMaxLength(150);
            b.Property(u => u.Telefono).IsRequired().HasMaxLength(20);
            b.Property(u => u.FechaRegistro).HasDefaultValueSql("SYSUTCDATETIME()");

            // Índice único: garantiza que no existan dos clientes con el mismo correo AUNQUE dos
            // peticiones lleguen al mismo tiempo. La validación en Flujo da el mensaje amable; el índice es la red de seguridad.
            b.HasIndex(u => u.Correo).IsUnique();
        }
    }
}
