using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public   class EventType
    {
        public int Id { get; set; }

        [Display(Name = "Evento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
        [Display(Name = "Activo")]
        public bool Active { get; set; }
        public ICollection<ItemPhoto>? ItemPhotos { get; set; } = null!;
    }
}
