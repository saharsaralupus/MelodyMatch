﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
    public class Contratante
    {
        public int Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "No se permiten más de 20 dígitos")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Document { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "No se permiten más de 50 Caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(12, ErrorMessage = "Número inválido")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [MaxLength(50, ErrorMessage = "No se permiten más de 50 Caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "Digite un Email válido")]
        public string Email { get; set; }

        [JsonIgnore]
        public ICollection<Reservacion> Reservaciones { get; } = new List<Reservacion>();

        [JsonIgnore]
        public ICollection<Calificacion_Contratante> Calificaciones_Contratante { get; } = new List<Calificacion_Contratante>();

        [JsonIgnore]
        public ICollection<Calificacion_Agrupacion> Calificaciones_Agrupacion { get; } = new List<Calificacion_Agrupacion>();
    }
}
