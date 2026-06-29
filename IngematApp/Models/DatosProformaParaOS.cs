namespace IngematApp.Models
{
    public class DatosProformaParaOS
    {
        public int IdProforma { get; set; }
        public string NomCliente { get; set; } = string.Empty;
        public string NDocumento { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string DireccionProforma { get; set; } = string.Empty;
        public decimal PrecioFinal { get; set; }
        public string NomCategoria { get; set; } = string.Empty;
        public string NomFormato { get; set; } = string.Empty;
    }
}