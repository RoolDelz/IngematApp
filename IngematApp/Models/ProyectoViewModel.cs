namespace IngematApp.Models
{
    public class ProyectoViewModel
    {
        public int IdProyecto { get; set; }
        public string NomProyecto { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int TotalOTs { get; set; }
        public int OTsCompletadas { get; set; }
    }

    public class ProyectoOTViewModel
    {
        public int IdOT { get; set; }
        public string N_OT { get; set; } = string.Empty;
        public string NomOT { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TecnicoAsignado { get; set; } = string.Empty;
    }

    public class ProyectoIndexViewModel
    {
        public List<ProyectoViewModel> Activos { get; set; } = new();
        public List<ProyectoViewModel> Finalizados { get; set; } = new();
    }
}
