using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TropiNailsPro.Models
{
    public class Usuario : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Número de teléfono no válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string? Telefono { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La clave no puede superar los 100 caracteres.")]
        public string? Clave { get; set; }

        [StringLength(50)]
        public string? UsuarioLogin { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // NUEVOS CAMPOS AGREGADOS 👇

        /// <summary>
        /// Rol del usuario. Ej: "Propietaria", "Cliente", "Administrador"
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = "Cliente";

        /// <summary>
        /// Indica si el correo electrónico fue confirmado (si usas validación por email).
        /// </summary>
        public bool CorreoConfirmado { get; set; } = false;

        /// <summary>
        /// Indica si el usuario está activo o bloqueado.
        /// </summary>
        public bool Activo { get; set; } = true;

        // Validación personalizada: exige al menos un medio de contacto
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Telefono))
            {
                yield return new ValidationResult(
                    "Debe ingresar al menos un correo electrónico o un número de teléfono.",
                    new[] { nameof(Email), nameof(Telefono) });
            }
        }
    }
}
