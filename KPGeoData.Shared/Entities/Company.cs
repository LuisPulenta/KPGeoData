﻿using System.ComponentModel.DataAnnotations;

namespace KPGeoData.Shared.Entities
{
    public   class Company
    {
        public int Id { get; set; }


        [Display(Name = "Empresa")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Domicilio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Phone { get; set; } = null!;

        [Display(Name = "CUIT")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string CUIT { get; set; } = null!;

        [Display(Name = "EMail")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string EMail { get; set; } = null!;

        [Display(Name = "Contacto")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Contact { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        //public ICollection<User> Users { get; set; }
    }
}