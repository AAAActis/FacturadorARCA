using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3pjxx.CodeFirst
{
    //El required significa que si o si lo pide, tmb le puse longitud a los valores
    //me parece que a "tipo" no lo tendriamos que poner como char
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

        public decimal ImporteTotal { get; set; }

        [ForeignKey("IdCliente")]
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        [NotMapped]
        public decimal Total => Items.Sum(i => i.SubTotal);
    }
}
