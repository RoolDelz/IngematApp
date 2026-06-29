using System.ComponentModel.DataAnnotations;

namespace IngematApp.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 dígitos")]
        public string Dni { get; set; } = string.Empty;

        public string TelefonoEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string CorreoEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe asignar un cargo/perfil")]
        public string Cargo { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}