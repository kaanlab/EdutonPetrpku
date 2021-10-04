using EdutonPetrpku.Data;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SurveyController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("savechoice")]
        public async Task<ActionResult<Nationality>> SaveChoice(SurveyModel surveyModel)
        {
            var nationality = _context.Nationalities.FirstOrDefault(n => n.Id == surveyModel.NationalityId);
            var appUser = await _userManager.FindByIdAsync(surveyModel.AppUserId);
            nationality.AppUser = new AppUser();
            nationality.AppUser = appUser;

            _context.Update(nationality);
            await _context.SaveChangesAsync();

            return Ok(nationality);
        }
    }
}
