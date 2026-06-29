using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IngematApp.Models
{
    public class Formato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFormato { get; set; }

        [Required(ErrorMessage = "El nombre del formato es obligatorio")]
        public string NomFormato { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal PrecioFormato { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategoria { get; set; }

        // Propiedad extra solo para la vista (no se guarda en la tabla Formato)
        public string? NombreCategoria { get; set; }
    }
}