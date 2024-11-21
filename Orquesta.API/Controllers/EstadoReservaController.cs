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
    [Route("/api/estadoReserva")]
    public class EstadoReservaController : ControllerBase
    {
        private readonly DataContext _context;

        public EstadoReservaController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.EstadoReservas.ToListAsync());
        }
    }
}
