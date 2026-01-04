using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parcial3pjxx.CodeFirst
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdItem { get; set; }

        [Required, MaxLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal ImporteUnitario { get; set; }

        [NotMapped]
        public decimal SubTotal => Cantidad * ImporteUnitario;

        [ForeignKey("IdFactura")]
        public int IdFactura { get; set; }
        public virtual Factura Factura { get; set; }
    }
}