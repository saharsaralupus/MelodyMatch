using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Orquesta.API.Data;
using Orquesta.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Orquesta.API.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/integrante")]

    public class IntegranteController : ControllerBase
    {
        public readonly DataContext _context;

        public IntegranteController(DataContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Integrantes.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Post(Integrante integrante)
        {
            _context.Add(integrante);
            await _context.SaveChangesAsync();
            return Ok(integrante);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var integrante = await _context.Integrantes.FirstOrDefaultAsync(x => x.Id == id);
            if(integrante == null)
            {
                return NotFound();
            }

            return Ok(integrante);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Integrante integrante)
        {
            _context.Update(integrante);
            await _context.SaveChangesAsync();
            return Ok(integrante);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var integrante = await _context.Integrantes
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (integrante == null)
            {
                return NotFound();
            }

            return NoContent();
        }
             
    }
}
