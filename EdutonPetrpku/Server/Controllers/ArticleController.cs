using EdutonPetrpku.Data;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class ArticleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ArticleController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Article>>> All()
        {
            var articles = await _context.Articles.Include(u => u.AppUser).ToListAsync();
            if (articles is not null)
            {
                return Ok(articles);
            }
            else
            {
                return Ok(Enumerable.Empty<Article>().ToArray());
            }
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("add")]
        public async Task<ActionResult<Article>> Add(Article article)
        {
            //TODO: add AppUser on the server side
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Id == article.AppUserId)
            {
                article.PublishDate = DateTime.Now;
                var newArticle = await _context.Articles.AddAsync(article);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok(newArticle.Entity);
                }
            }

            return BadRequest();
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPut("update")]
        public async Task<ActionResult<Article>> Update(Article article)
        {
            var articleToUpdate = await _context.Articles.FindAsync(article.Id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Id == article.AppUserId)
            {
                articleToUpdate.Title = article.Title;
                articleToUpdate.Content = article.Content;
                articleToUpdate.UpdateDate = DateTime.Now;

                var updatedArticle = _context.Articles.Update(articleToUpdate);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok(updatedArticle.Entity);
                }
            }

            return BadRequest();

        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var articleToDelete = await _context.Articles.FindAsync(id);

            _context.Articles.Remove(articleToDelete);
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
