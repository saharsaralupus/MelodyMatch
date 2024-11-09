using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
    public class Agrupacion
    {

        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(60, ErrorMessage = "No se permiten más de 60 Caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "El campo {0} es obligatorio")]
		public string Description { get; set; }
		[Required(ErrorMessage = "El campo {0} es obligatorio")]
		public int Price_Hour { get; set; }

        [JsonIgnore]
		public Representante Representante { get; set; }
		public int RepresentanteId { get; set; }


		[JsonIgnore]
        public ICollection<Integrante> Integrantes { get; } = new List<Integrante>();

        [JsonIgnore]
        public ICollection<Reservacion> Reservaciones { get; } = new List<Reservacion>();

    }
}
