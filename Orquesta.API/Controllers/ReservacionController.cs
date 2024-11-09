using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orquesta.API.Data;
using System.Threading.Tasks;
using Orquesta.Shared.Entities;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace Orquesta.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/reservacion")]
    public class ReservacionController : ControllerBase
    {
        private readonly DataContext _context;

        public ReservacionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Reservaciones.ToListAsync());
        }


        [HttpPost]
        public async Task<ActionResult> Post(Reservacion reservacion)
        {
   
            _context.Add(reservacion);
            await _context.SaveChangesAsync();
            return Ok(reservacion);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var reservacion = await _context.Reservaciones.FirstOrDefaultAsync(x => x.Id == id);
            if(reservacion == null)
            {
                return NotFound();
            }

            return Ok(reservacion);

        }

        [HttpPut]
        public async Task<ActionResult> Put(Reservacion reservacion)
        {
            _context.Update(reservacion);
            await _context.SaveChangesAsync();
            return Ok(reservacion);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var borrarReservacíon = await _context.Reservaciones
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            if(borrarReservacíon == null)
            {
                return NotFound("Reservación no encontrada");  
            }

            return NoContent();
        }

    }
}
