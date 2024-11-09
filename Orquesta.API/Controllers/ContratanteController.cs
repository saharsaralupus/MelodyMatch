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
    [Route("/api/contratante")]
    public class ContratanteController : ControllerBase
    {

        private readonly DataContext _context;

        public ContratanteController(DataContext context)
        {
            _context = context;
        }


        //Método GET --- Select * From Owners
        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.Contratantes.ToListAsync());
        }


        //Método POST- insertar en base de datos
        [HttpPost]

        public async Task<ActionResult> Post(Contratante contrante)
        {

            _context.Add(contrante);
            await _context.SaveChangesAsync();
            return Ok(contrante);
        }

        //GEt por párametro- select * from Owners where id=1
        [HttpGet("{id:int}")]

        public async Task<ActionResult> Get(int id)
        {

            var contratante = await _context.Contratantes.FirstOrDefaultAsync(x => x.Id == id);
            if (contratante == null)
            {


                return NotFound();  //404
            }

            return Ok(contratante);//200


        }


        //Método PUT- actualizar datos 
        [HttpPut]

        public async Task<ActionResult> Put( Contratante contratante)
        {
            
                _context.Update(contratante);
                await _context.SaveChangesAsync();
                return Ok(contratante);
        }

        //Delete - Eliminar registros

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {

            var borradoFilas = await _context.Contratantes
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
