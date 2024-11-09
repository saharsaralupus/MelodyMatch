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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/agrupacionGenero")]
    public class Agrupacion_Genero_Controller : ControllerBase
    {
        private readonly DataContext _context;

        public Agrupacion_Genero_Controller (DataContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Agrupacion_Generos.ToListAsync());
        }

        [HttpPost]

        public async Task<ActionResult> Post(Agrupacion_Genero agrupacion_Genero)
        {

            _context.Add(agrupacion_Genero);
            await _context.SaveChangesAsync();
            return Ok(agrupacion_Genero);

        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {
            var agrupacionGenero = await _context.Agrupacion_Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (agrupacionGenero == null)
            {
                return NotFound();
            }

            return Ok(agrupacionGenero);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Agrupacion_Genero agrupacion_Genero)
        {
            _context.Update(agrupacion_Genero);
            await _context.SaveChangesAsync();
            return Ok(agrupacion_Genero);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var borrarAgrupacionGenero = await _context.Agrupacion_Generos
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (borrarAgrupacionGenero == null)
            {
                return NotFound();
            }

            return NoContent();


        }



    }
}
