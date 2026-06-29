using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IngematApp.Models
{
    public class SubFormato
    {
        [Key]
        public int IdSubFormato { get; set; }

        [Required(ErrorMessage = "El nombre del subformato es obligatorio")]
        public string NomSubFormato { get; set; } = string.Empty;

        // No pedimos Precio aquí porque en tu base de datos SQL el precio no está en Subformato

        [Required(ErrorMessage = "Debe seleccionar un formato padre")]
        public int IdFormato { get; set; }

        // Propiedades extra exclusivas para mostrar en la tabla (no se guardan en BD)
        public string? NombreFormato { get; set; }
        public string? NombreCategoria { get; set; }
    }
}