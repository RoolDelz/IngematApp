using System.ComponentModel.DataAnnotations;

namespace IngematApp.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NomCliente { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;
        public string NDocumento { get; set; } = string.Empty;
        public string TelefonoCliente { get; set; } = string.Empty;
        public string CorreoCliente { get; set; } = string.Empty;
    }
}