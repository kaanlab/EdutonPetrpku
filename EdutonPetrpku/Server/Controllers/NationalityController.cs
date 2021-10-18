using EdutonPetrpku.Data;
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
using System.Text.Json;
using System.Text.Json.Serialization;


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
        public async Task<IEnumerable<NationalityViewModel>> All()
        {
            var nationalities = await _context.Nationalities.Include(u => u.AppUser).ToListAsync();

            List<NationalityViewModel> nationalityViewModels = new List<NationalityViewModel>();

            foreach (var nationality in nationalities)
            {
                nationalityViewModels.Add(nationality.ToNationalityViewModel()); 
            }

            return nationalityViewModels;

        }

        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("add")]
        public async Task<ActionResult<Nationality>> Add(NationalityViewModel nationalityViewModel)
        {
            var nationality = new Nationality()
            {
                Name = nationalityViewModel.Name,
                Population = nationalityViewModel.Population,
                Subject = nationalityViewModel.Subject
            };
            
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
        public async Task<ActionResult<NationalityViewModel>> Update(NationalityViewModel nationalityViewModel)
        {
            var nationalityToUpdate = await _context.Nationalities
                .Include(u => u.AppUser)
                .FirstAsync(n => n.Id == nationalityViewModel.Id);

            nationalityToUpdate.Name = nationalityViewModel.Name;
            nationalityToUpdate.Subject = nationalityViewModel.Subject;
            nationalityToUpdate.Population = nationalityViewModel.Population;

            var updatedNationality = _context.Nationalities.Update(nationalityToUpdate);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(updatedNationality.Entity.ToNationalityViewModel());
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
