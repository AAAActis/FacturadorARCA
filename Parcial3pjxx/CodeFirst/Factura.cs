using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Parcial3pjxx.CodeFirst
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }

        [Required]
        public char Tipo { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        // SQLite maneja decimales, pero a veces requiere conversión. EF Core suele encargarse.
        public decimal ImporteTotal { get; set; }

        [ForeignKey("IdCliente")]
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        [NotMapped]
        public decimal Total
        {
            get
            {
                if (Items == null) return 0;
                return Items.Sum(i => i.SubTotal);
            }
        }
    }
}