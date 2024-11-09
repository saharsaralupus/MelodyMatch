using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Orquesta.Shared.Entities
{
    public class GeneroMusical
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(60, ErrorMessage = "No se permiten más de 60 Caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }
    }
}
