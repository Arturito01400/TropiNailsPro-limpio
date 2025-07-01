using System;
using System.ComponentModel.DataAnnotations;

namespace TropiNailsPro.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [Required]
        public string ClienteNombre { get; set; } = string.Empty;

        [Required]
        public string ModeloUnas { get; set; } = string.Empty;

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public string TransaccionId { get; set; } = string.Empty;

        public DateTime FechaPago { get; set; } = DateTime.Now;
    }
}
