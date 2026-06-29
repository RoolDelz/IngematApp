namespace IngematApp.Models
{
    public class FacturaViewModel
    {
        public int IdFactura { get; set; }
        public string NumFactura { get; set; } = string.Empty;
        public DateTime FechaFactura { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFactura { get; set; }
        public string NomProyecto { get; set; } = string.Empty;
        public string NomCliente { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
    }

    public class FacturaIndexViewModel
    {
        public List<FacturaViewModel> Pendientes { get; set; } = new();
        public List<FacturaViewModel> Realizadas { get; set; } = new();
        public List<FacturaViewModel> Pagadas { get; set; } = new();
    }

    public class FacturaImpresion
    {
        public int IdFactura { get; set; }
        public string NumFactura { get; set; } = string.Empty;
        public DateTime FechaFactura { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFactura { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        // Proyecto
        public string NomProyecto { get; set; } = string.Empty;
        // Cliente
        public string NomCliente { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string NDocumento { get; set; } = string.Empty;
        public string TelefonoCliente { get; set; } = string.Empty;
        public string CorreoCliente { get; set; } = string.Empty;
        // Empresa
        public string NomEmpresa { get; set; } = string.Empty;
        public string Ruc { get; set; } = string.Empty;
        public string DireccionEmpresa { get; set; } = string.Empty;
        // Servicio
        public string Motivo { get; set; } = string.Empty;
        public string NomCategoria { get; set; } = string.Empty;
        public string NomFormato { get; set; } = string.Empty;
    }
}
