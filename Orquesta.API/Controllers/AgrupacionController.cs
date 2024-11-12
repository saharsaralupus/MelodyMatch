using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orquesta.API.Data;
using System.Threading.Tasks;
using Orquesta.Shared.Entities;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Orquesta.API.Controllers
{
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/agrupacion")]
    public class AgrupacionController : ControllerBase
    {
        private readonly DataContext _context;

        public AgrupacionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet] //Obtener datos
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Agrupaciones.ToListAsync());
        }

        [HttpPost] //Insertar datos nuevos 

        public async Task<ActionResult> Post(Agrupacion agrupacionSolista)
        {

            _context.Add(agrupacionSolista);
            await _context.SaveChangesAsync();
            return Ok(agrupacionSolista);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {
            var agrupacionSolista = await _context.Agrupaciones.FirstOrDefaultAsync(x => x.Id == id);
            if (agrupacionSolista == null)
            {
                return NotFound("Agrupacion no encontrada");
            }

            return Ok(agrupacionSolista);
        }

        [HttpPut] //Modificar datos

        public async Task<ActionResult> Put(Agrupacion agrupacionSolista)
        {
            _context.Update(agrupacionSolista);
            await _context.SaveChangesAsync();
            return Ok(agrupacionSolista);
        }

        [HttpDelete("{id:int}")]
         public async Task<ActionResult> Delete(int id)
        {
            var borrarMusico = await _context.Agrupaciones
				.Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if(borrarMusico == null)
            {
                return NotFound("Agrupación no encontrada");
            }
            return NoContent();

        }
    }
}
