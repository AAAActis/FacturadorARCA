using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3pjxx.CodeFirst
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }
        [Required]
        [MaxLength(11)]
        public long CuilCuit { get; set; }
        [Required]
        [MaxLength(200)]
        public string RazonSocial { get; set; }
        [MaxLength(200)]

        public string Domicilio { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

        public Cliente()
        {

        }

        public Cliente(long cuil, string razon, string domicilio)
        {
            CuilCuit = cuil;
            RazonSocial = razon;
            Domicilio = domicilio;
        }
    }
}

