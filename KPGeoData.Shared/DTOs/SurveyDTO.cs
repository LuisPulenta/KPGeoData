using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.DTOs
{
    public class SurveyDTO
    {
        public int Id { get; set; }

        [Display(Name = "Relevamiento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public List<int>? StateIds { get; set; }
        public List<int>? EventTypeIds { get; set; }
        public List<int>? ItemTypeIds { get; set; }
    }
}
