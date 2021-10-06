using EdutonPetrpku.Data;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitePageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SitePageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<SitePage[]>> All() =>
              await _context.SitePages.OrderBy(o => o.Order).ToArrayAsync();

        [HttpGet("main")]
        public async Task<ActionResult<SitePage>> GetMainPage() =>
            await _context.SitePages.FirstOrDefaultAsync(p => p.Order == 1);

        [HttpGet("{url}")]
        public async Task<ActionResult<SitePage>> GetPage(string url) =>        
            await _context.SitePages.FirstOrDefaultAsync(p => p.Url == url);
        

        [HttpPost("add")]
        public async Task<ActionResult<Nationality>> Add(SitePage sitePage)
        {
            var newSitePage = await _context.SitePages.AddAsync(sitePage);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(newSitePage.Entity);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<SitePage>> Update(SitePage sitePage)
        {
            var sitePageToUpdate = await _context.SitePages.FirstOrDefaultAsync(n => n.Id == sitePage.Id);

            sitePageToUpdate.Order = sitePage.Order;
            sitePageToUpdate.Url = sitePage.Url;
            sitePageToUpdate.Name = sitePage.Name;
            sitePageToUpdate.Content = sitePage.Content;

            var updatedSitePage = _context.SitePages.Update(sitePageToUpdate);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(updatedSitePage.Entity);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete/{sitePageId}")]
        public async Task<ActionResult> Delete(string sitePageId)
        {
            var sitePageToDelete = await _context.SitePages.FirstOrDefaultAsync(n => n.Id == int.Parse(sitePageId));

            var deletedNationality = _context.SitePages.Remove(sitePageToDelete);
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
