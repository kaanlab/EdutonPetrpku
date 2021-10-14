using EdutonPetrpku.Data;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Authorization;
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
    public class ArticleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArticleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<Article[]>> All()
        {
            var articles = await _context.SitePages.OrderBy(o => o.Order).ToArrayAsync();
            if (articles is not null)
            {
                return Ok(articles);
            }
            else
            {
                return Ok(Enumerable.Empty<SitePage>().ToArray());
            }
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("add")]
        public async Task<ActionResult<Article>> Add(Article article)
        {
            article.PublishDate = DateTime.Now;
            //TODO: add AppUser on the server side
            var newArticle = await _context.Articles.AddAsync(article);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(newArticle.Entity);
            }
            else
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPut("update")]
        public async Task<ActionResult<Article>> Update(Article article)
        {
            var articleToUpdate = await _context.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);

            articleToUpdate.Title = article.Title;
            articleToUpdate.Content = article.Content;
            articleToUpdate.UpdateDate = DateTime.Now;

            var updatedArticle = _context.Articles.Update(articleToUpdate);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(updatedArticle.Entity);
            }
            else
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpDelete("delete/{int:articleId}")]
        public async Task<ActionResult> Delete(int articleId)
        {
            var articleToDelete = await _context.Articles.FirstOrDefaultAsync(a => a.Id == articleId);

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
