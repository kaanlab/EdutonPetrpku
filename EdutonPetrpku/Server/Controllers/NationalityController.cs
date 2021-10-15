﻿using EdutonPetrpku.Data;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace EdutonPetrpku.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NationalityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Nationality>>> All() =>
             await _context.Nationalities.Include(u => u.AppUser).ToListAsync();


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("add")]
        public async Task<ActionResult<Nationality>> Add(Nationality nationality)
        {
            var newNationality = await _context.Nationalities.AddAsync(nationality);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(newNationality.Entity);
            }
            else
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPut("update")]
        public async Task<ActionResult<Nationality>> Update(Nationality nationality)
        {
            var nationalityToUpdate = await _context.Nationalities.FirstOrDefaultAsync(n => n.Id == nationality.Id);

            nationalityToUpdate.Name = nationality.Name;
            nationalityToUpdate.Subject = nationality.Subject;
            nationalityToUpdate.Population = nationality.Population;

            var updatedNationality = _context.Nationalities.Update(nationalityToUpdate);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(updatedNationality.Entity);
            }
            else
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var nationalityToDelete = await _context.Nationalities.FindAsync(id);

            _context.Nationalities.Remove(nationalityToDelete);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
