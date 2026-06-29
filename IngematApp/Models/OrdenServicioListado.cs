using System;

namespace IngematApp.Models
{
    public class OrdenServicioListado
    {
        public int IdOS { get; set; }
        public DateTime FechaOS { get; set; }
        public string NomCliente { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string NomEmpresa { get; set; } = string.Empty;
        public string EstadoOS { get; set; } = string.Empty;
        public decimal PrecioFinal { get; set; }
    }
}