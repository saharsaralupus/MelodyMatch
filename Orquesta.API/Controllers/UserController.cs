﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orquesta.API.Data;
using Orquesta.Shared.Entities;
using System;
using System.Linq;
using Orquesta.API.Helpers;
using Orquesta.Shared.DTOs;
using System.Threading.Tasks;

namespace Orquesta.API.Controllers
{



    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/Users")]
    public class UserControllers : ControllerBase
    {

        private readonly DataContext _context;

        //Constructor
        public UserControllers(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {

            var queryable = _context.Users
             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Email.ToLower().Contains(pagination.Filter.ToLower()));
            }
            return Ok(await queryable
            .OrderBy(x => x.Id)
            .Paginate(pagination)
            .ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)

        {
            var queryable = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Email.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }


        //Method Create
        [HttpPost]
        public async Task<ActionResult> PostAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        //Method Get by ID (Read)
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync
                (x => x.Id.Equals(id));

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //Method Update
        [HttpPut]
        public async Task<ActionResult> PutAsync(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        //Metod Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync
                  (x => x.Id.Equals(id));

            if (user == null)
            {
                return NotFound();
            }
            _context.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Users.ToListAsync());
        }
    }
}
