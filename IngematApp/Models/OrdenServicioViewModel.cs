using System.ComponentModel.DataAnnotations;

namespace IngematApp.Models
{
    public class OrdenServicioViewModel
    {
        // --- DATOS INFORMATIVOS (Proforma) ---
        public int IdProforma { get; set; }
        public string NomCliente { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public decimal PrecioFinal { get; set; }

        // --- DATOS A LLENAR (Nueva Empresa) ---
        [Required(ErrorMessage = "La razón social de la empresa es obligatoria")]
        public string NomEmpresa { get; set; } = string.Empty;

        [Required(ErrorMessage = "El RUC es obligatorio")]
        public string Ruc { get; set; } = string.Empty;

        public string TelefonoEmpresa { get; set; } = string.Empty;
        public string CorreoEmpresa { get; set; } = string.Empty;
        public string DireccionEmpresa { get; set; } = string.Empty;
    }
}