using Abstracciones.Usuarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DA.Contexto
{
    public class BibliotecaContexto : DbContext
    {
        public BibliotecaContexto(DbContextOptions<BibliotecaContexto> options) : base(options) { }

        public DbSet<Usuario> Clientes => Set<Usuario>();
    //    public DbSet<Libro> Tickets => Set<Libro>();
      //  public DbSet<Comentario> Comentarios => Set<Comentario>();
        //public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();
     //   public DbSet<PrestamoLibro> Agentes => Set<PrestamoLibro>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ****PROFESOR: **** Una clase de configuración por entidad, junto a su módulo (carpeta).
            // Esta línea las descubre todas: agregar un módulo no obliga a tocar este archivo.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaContexto).Assembly);
        }
    }

}

