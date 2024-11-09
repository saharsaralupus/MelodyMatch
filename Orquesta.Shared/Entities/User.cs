using Orquesta.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Orquesta.Shared.Entities

{
    public class User : IdentityUser

    {

        [MaxLength(20, ErrorMessage = "No se permiten más de 20 dígitos")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Document { get; set; }

        [MaxLength(50, ErrorMessage = "No se permiten más de 50 caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "No se permiten más de 50 caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string LastName { get; set; }

        [Display(Name = "Foto")]
        public string Photo { get; set; }
        public UserType UserType { get; set; }
        public string FullName => $"{FirstName} {LastName}";

    }

}