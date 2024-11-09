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
    [Route("/api/representante")]
    public class RepresentanteController : ControllerBase
    {

        private readonly DataContext _context;

        public RepresentanteController(DataContext context)
        {
            _context = context;
        }


        //Método GET --- Select * From Owners
        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.Representantes.ToListAsync());
        }


        //Método POST- insertar en base de datos
        [HttpPost]

        public async Task<ActionResult> Post(Representante representante)
        {

            _context.Add(representante);
            await _context.SaveChangesAsync();
            return Ok(representante);
        }

        //GEt por párametro- select * from Owners where id=1
        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {

            var representante = await _context.Representantes.FirstOrDefaultAsync(x => x.Id == id);
            if (representante == null)
            {


                return NotFound();  //404
            }

            return Ok(representante);//200


        }


        //Método PUT- actualizar datos 
        [HttpPut]

        public async Task<ActionResult> Put(Representante representante)
        {

            _context.Update(representante);
            await _context.SaveChangesAsync();
            return Ok(representante);
        }

        //Delete - Eliminar registros

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {

            var borradoFilas = await _context.Representantes
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
