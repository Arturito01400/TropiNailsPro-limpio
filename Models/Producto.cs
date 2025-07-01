using System;
using System.ComponentModel.DataAnnotations;

namespace TropiNailsPro.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser 0 o mayor.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        // ✅ Campo adicional: Fecha de creación
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
