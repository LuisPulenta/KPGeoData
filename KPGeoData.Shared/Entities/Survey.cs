using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public   class Survey
    {
        public int Id { get; set; }

        [Display(Name = "Relevamiento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
        
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        [Display(Name = "Activo")]
        public bool Active { get; set; }

        public ICollection<Item>? Items { get; set; }

        [Display(Name = "Items")]
        public int ItemsNumber => Items == null ? 0 : Items.Count;
    }
}
