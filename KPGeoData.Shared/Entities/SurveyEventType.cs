using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public class SurveyEventType
    {
        public int Id { get; set; }

        public Survey? Survey { get; set; }
        public int SurveyId { get; set; }

        [Display(Name = "Evento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public EventType EventType { get; set; } = null!;
    }
}
