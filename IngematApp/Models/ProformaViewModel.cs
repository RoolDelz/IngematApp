using System.ComponentModel.DataAnnotations;

namespace IngematApp.Models
{
    public class ProformaViewModel
    {
        // --- DATOS DEL CLIENTE ---
        [Required(ErrorMessage = "El nombre de la empresa/cliente es obligatorio")]
        public string NomCliente { get; set; } = string.Empty;
        public string Documento { get; set; } = "RUC"; // RUC o DNI
        public string NDocumento { get; set; } = string.Empty;
        public string TelefonoCliente { get; set; } = string.Empty;
        public string CorreoCliente { get; set; } = string.Empty;

        // --- DATOS DE LA PROFORMA ---
        [Required(ErrorMessage = "El motivo es obligatorio")]
        public string Motivo { get; set; } = string.Empty;
        public string DireccionProforma { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un formato")]
        public int IdFormato { get; set; }

        // No pedimos el total en el formulario porque lo calcularemos en el servidor
        // usando el precio del Formato y aplicándole el 18% de IGV




    }
}