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
	[Route("/api/repertorio")]
	public class RepertorioController : ControllerBase
	{
		public readonly DataContext _context;

		public RepertorioController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult> Get()
		{
			return Ok(await _context.Repertorios.ToListAsync());
		}

		[HttpPost]
		public async Task<ActionResult> Post(Repertorio repertorio)
		{
			_context.Add(repertorio);
			await _context.SaveChangesAsync();
			return Ok(repertorio);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult> Get(int id)
		{
			var repertorio = await _context.Repertorios.FirstOrDefaultAsync(x => x.Id == id);
			if (repertorio == null)
			{
				return NotFound();
			}

			return Ok(repertorio);
		}

		[HttpPut]
		public async Task<ActionResult> Put(Repertorio repertorio)
		{
			_context.Update(repertorio);
			await _context.SaveChangesAsync();
			return Ok(repertorio);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
		{
			var repertorio = await _context.Repertorios
				.Where(x => x.Id == id)
				.ExecuteDeleteAsync();

			if (repertorio == null)
			{
				return NotFound();
			}

			return NoContent();
		}

	}
}
