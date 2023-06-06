using KPGeoData.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using KPGeoData.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        public Company? Company { get; set; }

        [Display(Name = "Empresa")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int CompanyId { get; set; }
        
        [Display(Name = "Activo")]
        public bool Active { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
