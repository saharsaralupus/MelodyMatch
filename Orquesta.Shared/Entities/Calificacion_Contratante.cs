using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
    public class Calificacion_Contratante
    {
        public int Id { get; set; }

        [Display(Name = "Puntaje")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, 5, ErrorMessage = "El campo {0} debe estar entre 1 y 5")]
        public int Puntaje { get; set; }

        [Display(Name = "Comentario")]
        [MaxLength(200, ErrorMessage = "No se permiten más de 200 Caracteres")]
        public string Comentario { get; set; }

        [JsonIgnore]
        public Contratante Contratante { get; set; }
        public int ContratanteId { get; set; }

    }
}
