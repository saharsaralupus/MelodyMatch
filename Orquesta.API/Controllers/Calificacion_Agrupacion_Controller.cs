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
    [Route("/api/calificacion_agrupacion")]
    public class Calificacion_Agrupacion_Controller : ControllerBase
    {

        private readonly DataContext _context;

        public Calificacion_Agrupacion_Controller(DataContext context)
        {
            _context = context;
        }

        [HttpGet] //Obtener datos
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Calificaciones_Agrupacion.ToListAsync());
        }

        [HttpPost] //Insertar datos nuevos 

        public async Task<ActionResult> Post(Calificacion_Agrupacion calificacionAgrupacion)
        {

            _context.Add(calificacionAgrupacion);
            await _context.SaveChangesAsync();
            return Ok(calificacionAgrupacion);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {
            var calificacionAgrupacion = await _context.Calificaciones_Agrupacion.FirstOrDefaultAsync(x => x.Id == id);
            if (calificacionAgrupacion == null)
            {
                return NotFound("Calificación no encontrada");
            }

            return Ok(calificacionAgrupacion);
        }

        [HttpPut] //Modificar datos

        public async Task<ActionResult> Put(Calificacion_Agrupacion calificacionAgrupacion)
        {
            _context.Update(calificacionAgrupacion);
            await _context.SaveChangesAsync();
            return Ok(calificacionAgrupacion);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var borrarCalificacion = await _context.Calificaciones_Agrupacion
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
