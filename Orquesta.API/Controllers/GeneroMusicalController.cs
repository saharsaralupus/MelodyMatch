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
    [Route("/api/generoMusical")]
    public class GeneroMusicalController : ControllerBase
    {
        private readonly DataContext _context;

        public GeneroMusicalController(DataContext context)
        {
            _context = context;
        }


        //Método GET --- Select * From Owners
        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.GeneroMusicales.ToListAsync());
        }


        //Método POST- insertar en base de datos
        [HttpPost]

        public async Task<ActionResult> Post(GeneroMusical generoMusical)
        {

            _context.Add(generoMusical);
            await _context.SaveChangesAsync();
            return Ok(generoMusical);
        }

        //GEt por párametro- select * from Owners where id=1
        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {

            var generoMusical = await _context.GeneroMusicales.FirstOrDefaultAsync(x => x.Id == id);
            if (generoMusical == null)
            {


                return NotFound();  //404
            }

            return Ok(generoMusical);//200


        }


        //Método PUT- actualizar datos 
        [HttpPut]

        public async Task<ActionResult> Put(GeneroMusical generoMusical)
        {

            _context.Update(generoMusical);
            await _context.SaveChangesAsync();
            return Ok(generoMusical);
        }

        //Delete - Eliminar registros

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {

            var borradoFilas = await _context.GeneroMusicales
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (borradoFilas == 0)
            {


                return NotFound();  //404
            }

            return NoContent();//204


        }
    }
}
