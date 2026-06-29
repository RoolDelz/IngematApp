namespace IngematApp.Models
{
    public class OrdenTrabajoViewModel
    {
        public int IdOT { get; set; }
        public string N_OT { get; set; } = string.Empty;
        public string NomOT { get; set; } = string.Empty;
        public string NomProyecto { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TecnicoAsignado { get; set; } = string.Empty;
    }

    public class EmpleadoViewModel
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
    }
}
