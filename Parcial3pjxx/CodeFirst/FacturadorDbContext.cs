using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}