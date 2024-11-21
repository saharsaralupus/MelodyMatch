using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
    public class Reservacion
    {

        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [Required(ErrorMessage = "El campo {0} es obligaptorio")]
        public TimeSpan StartTime { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public TimeSpan FinalTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateOnly Fecha { get; set; }

        [Display(Name = "Lugar")]
        [MaxLength(100, ErrorMessage = "No se permiten más de 100 dcaracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Place { get; set; }

        [JsonIgnore]
        public EstadoReserva EstadoReserva { get; set; }
        public int EstadoReservaId { get; set; }

        [JsonIgnore]
		public Contratante Contratante { get; set; }
        public int ContratanteId { get; set; }

		[JsonIgnore]
		public Agrupacion Agrupacion { get; set; }
        public int AgrupacionId { get;set; }


    }
}
