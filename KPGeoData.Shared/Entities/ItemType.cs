using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public   class ItemType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo Item")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
        [Display(Name = "Activo")]
        public bool Active { get; set; }
    }
}
