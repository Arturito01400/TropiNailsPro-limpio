using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TropiNailsPro.Models
{
    public class ModeloUnas
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del modelo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 10000, ErrorMessage = "Debe ingresar un precio válido")]
        public decimal Precio { get; set; }

        // ✅ Campo para la URL pegada (Pinterest, WhatsApp, etc.)
        [Url(ErrorMessage = "Debe ingresar una URL válida")]
        [Display(Name = "URL de la Imagen (opcional)")]
        public string? ImagenUrl { get; set; }

        // ✅ Campo solo para la imagen subida desde la laptop
        [NotMapped]
        [Display(Name = "Imagen desde archivo (opcional)")]
        public IFormFile? ImagenArchivo { get; set; }

        // ✅ Ruta definitiva que se usará para mostrar la imagen en la app
        [Display(Name = "Ruta Final de Imagen")]
        public string? ImagenFinalRuta { get; set; }
    }
}
