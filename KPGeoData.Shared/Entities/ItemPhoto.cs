using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public class ItemPhoto
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; } = null!;
        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; } = null!;

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Observaciones")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Remarks { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public string PhotoFullPath => string.IsNullOrEmpty(Photo)
        ? $"http://190.111.249.225/RowingAppApi/images/Obras/noimage.png"
        : $"http://190.111.249.225/RowingAppApi{Photo[1..]}";
    }
}
