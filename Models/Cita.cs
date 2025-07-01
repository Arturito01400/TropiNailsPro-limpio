using System.ComponentModel.DataAnnotations;

namespace TropiNailsPro.Models
{
    public class Cita
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la clienta es obligatorio")]
        public string NombreClienta { get; set; } = null!;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        public string Servicio { get; set; } = null!;

        public string? NotasAdicionales { get; set; }
    }
}
