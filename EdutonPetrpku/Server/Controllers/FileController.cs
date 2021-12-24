using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        //[Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("upload")]
        public ActionResult UploadImg(IFormFile img)
        {
            var url = Path.Combine("upload", "ckeditor", img.FileName);
            var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, url);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/{url}");

            return Ok(new { url = resourcePath.AbsoluteUri });
        }

        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("avatar")]
        public ActionResult<UploadFileViewModel> UploadAvatar([FromForm] IEnumerable<IFormFile> files)
        {
            var file = files.FirstOrDefault();
            var url = Path.Combine("upload", file.FileName);
            var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, url);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }            

            return Ok(new UploadFileViewModel { Url = url });
        }

        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("diploma")]
        public ActionResult<UploadFileViewModel> UploadDiploma([FromForm] IEnumerable<IFormFile> files)
        {
            var file = files.FirstOrDefault();
            var url = Path.Combine("upload", "diplomas", file.FileName);
            var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, url);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok(new UploadFileViewModel { Url = url });
        }
    }
}
