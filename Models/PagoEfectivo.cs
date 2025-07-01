using System;
using System.ComponentModel.DataAnnotations;

namespace TropiNailsPro.Models
{
    public class PagoEfectivo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(1, 1000000, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [Display(Name = "Fecha de Pago")]
        [DataType(DataType.DateTime)]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [StringLength(255, ErrorMessage = "Las notas no pueden superar los 255 caracteres.")]
        public string? Notas { get; set; }

        // NUEVO CAMPO opcional para registrar método o caja si lo deseas en el futuro
        [Display(Name = "Método o Caja")]
        public string? Metodo { get; set; }
    }
}
