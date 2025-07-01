using System;
using System.ComponentModel.DataAnnotations;

namespace TropiNailsPro.Models
{
    public class PagoTransferencia
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El banco de origen es obligatorio.")]
        [StringLength(100, ErrorMessage = "El banco no puede exceder 100 caracteres.")]
        public string BancoOrigen { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(1, 1000000, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El número de referencia es obligatorio.")]
        [StringLength(100, ErrorMessage = "El número de referencia no puede exceder 100 caracteres.")]
        public string NumeroReferencia { get; set; } = string.Empty;

        public string? ImagenComprobante { get; set; }  // Ruta o URL de la imagen

        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        // NUEVAS PROPIEDADES AGREGADAS 👇

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre del modelo no puede exceder 200 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        public string? PayPalId { get; set; } // Se llena si el pago fue vía PayPal
    }
}
