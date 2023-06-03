using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public   class Item
    {
        public int Id { get; set; }

        public Survey? Survey { get; set; }
        public string State { get; set; } = null!;
        public string ItemType { get; set; } = null!;
        public int SurveyId { get; set; }

        [Display(Name = "Item")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Domicilio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Localidad")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Locality { get; set; } = null!;

        [Display(Name = "País")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Country { get; set; } = null!;
        [Display(Name = "Latitud")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public double Latitude { get; set; }

        [Display(Name = "Longitud")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public double Longitude { get; set; }

        //public User User { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }


        [Display(Name = "Observaciones")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Remarks { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        public ICollection<ItemPhoto>? ItemPhotos { get; set; }
        public int ItemPhotosNumber => ItemPhotos == null ? 0 : ItemPhotos.Count;

    }
}
