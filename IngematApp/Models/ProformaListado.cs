using System;

namespace IngematApp.Models
{
    public class ProformaListado
    {
        public int IdProforma { get; set; }
        public string NomCliente { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaP { get; set; }
        public decimal Total { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}