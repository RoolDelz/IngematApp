using System;

namespace IngematApp.Models
{
    public class ProformaImpresion
    {
        public int IdProforma { get; set; }
        public DateTime FechaP { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string DireccionProforma { get; set; } = string.Empty;

        public string NomCliente { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string NDocumento { get; set; } = string.Empty;
        public string TelefonoCliente { get; set; } = string.Empty;
        public string CorreoCliente { get; set; } = string.Empty;

        public string NomCategoria { get; set; } = string.Empty;
        public string NomFormato { get; set; } = string.Empty;

        public decimal Total { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}