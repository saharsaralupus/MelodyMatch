using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orquesta.API.Data;
using Orquesta.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Orquesta.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/califiacion_contratante")]
    public class Calificacion_Contratante_Controller : ControllerBase
    {

        private readonly DataContext _context;

        public Calificacion_Contratante_Controller(DataContext context)
        {
            _context = context;
        }

        [HttpGet] //Obtener datos
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Calificaciones_Contratante.ToListAsync());
        }

        [HttpPost] //Insertar datos nuevos 
        public async Task<ActionResult> Post(Calificacion_Contratante calificacionContratante)
        {

            _context.Add(calificacionContratante);
            await _context.SaveChangesAsync();
            return Ok(calificacionContratante);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {
            var calificacionContratante = await _context.Calificaciones_Contratante.FirstOrDefaultAsync(x => x.Id == id);
            if (calificacionContratante == null)
            {
                return NotFound("Calificación no encontrada");
            }

            return Ok(calificacionContratante);
        }

        [HttpPut] //Modificar datos

        public async Task<ActionResult> Put(Calificacion_Contratante calificacionContratante)
        {
            _context.Update(calificacionContratante);
            await _context.SaveChangesAsync();
            return Ok(calificacionContratante);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var borrarCalificacion = await _context.Calificaciones_Contratante
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (borrarCalificacion == null)
            {
                return NotFound("Calificación no encontrada");
            }
            return NoContent();

        }

    }
}
