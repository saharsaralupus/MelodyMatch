using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
	public class Repertorio
	{
		public int Id { get; set; }

		[Display(Name = "Nombre cación")]
		[Required(ErrorMessage = "El campo {0} es obligatorio")]
		public string Name { get; set; }

		[Display(Name = "Versión")]
		[Required(ErrorMessage = "El campo {0} es obligatorio")]
		public string Version { get; set; }

		[JsonIgnore]
		public Agrupacion Agrupacion { get; set; }
		public int AgrupacionId { get; set; }
	}
}
