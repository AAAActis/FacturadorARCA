using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3pjxx.CodeFirst
{
    public class FacturadorDbContext : DbContext
    {
        public FacturadorDbContext()
        {
        }

        public FacturadorDbContext(DbContextOptions<FacturadorDbContext> options)
            : base(options)
        {
        }

        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Factura>();
            modelBuilder.Entity<Item>();
            modelBuilder.Entity<Cliente>();
        }

        // Método añadido para garantizar la configuración de SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Intenta leer el archivo de configuración appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DbContext");

                // Si la cadena está vacía (o no leyó el archivo), usa un valor por defecto para SQLite
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Data Source=app.db";
                }

                // Configura el contexto para usar SQLite
                optionsBuilder.UseSqlite(connectionString);
            }
        }
    }
}