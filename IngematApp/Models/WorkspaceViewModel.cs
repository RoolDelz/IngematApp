namespace IngematApp.Models
{
    public class ReporteViewModel
    {
        public int IdReporte { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public DateTime FechaSubida { get; set; }
    }

    public class WorkspaceViewModel
    {
        public int IdOT { get; set; }
        public string N_OT { get; set; } = string.Empty;
        public string NomOT { get; set; } = string.Empty;
        public int IdProyecto { get; set; }
        public string NomProyecto { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        // Propiedad para el TextBox (El archivo .txt)
        public string ComentariosTexto { get; set; } = string.Empty;

        // Lista de archivos adjuntos
        public List<ReporteViewModel> Archivos { get; set; } = new List<ReporteViewModel>();
    }
}
