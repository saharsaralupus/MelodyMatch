using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
    public class Agrupacion_Genero
    {
        public int Id { get; set; }

		[JsonIgnore]
		public Agrupacion Agrupacion { get; set; }
		public int AgrupacionId { get; set; }
		[JsonIgnore]
		public GeneroMusical GeneroMusical { get; set; }
		public int GeneroMusicalId{ get; set; }

    }
}
