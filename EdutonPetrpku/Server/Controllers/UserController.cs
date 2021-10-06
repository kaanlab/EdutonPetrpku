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
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<AppUser[]>> All() =>
            await _userManager.Users.ToArrayAsync();


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("add")]
        public async Task<ActionResult<AppUser>> Add(AppUserModel userModel)
        {
            var appUser = new AppUser
            {
                DisplayName = userModel.DisplayName,
                UserName = userModel.UserName,
                Email = userModel.Email,
                Image = userModel.Image
            };
            await _userManager.CreateAsync(appUser, userModel.Password);
            await _userManager.AddToRoleAsync(appUser, GlobalVarables.Roles.USER);

            return Ok(appUser);
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPut("update")]
        public async Task<ActionResult<AppUser>> Update(AppUserUpdateModel userModel)
        {
            var appUser = await _userManager.FindByNameAsync(userModel.UserName);

            appUser.UserName = userModel.UserName;
            appUser.Email = userModel.Email;
            appUser.Image = userModel.Image;
            appUser.DisplayName = userModel.DisplayName;

            await _userManager.UpdateAsync(appUser);

            return Ok(appUser);
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpPost("changepw")]
        public async Task<ActionResult<AppUser>> ChangePassword(AppUserChPassModel userModel)
        {
            var appUser = await _userManager.FindByNameAsync(userModel.UserName);

            var result = await _userManager.ChangePasswordAsync(appUser, userModel.CurrentPassword, userModel.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest();
        }


        [Authorize(Roles = GlobalVarables.Roles.ADMIN)]
        [HttpDelete("delete/{usertodelete}")]
        public async Task<ActionResult> Delete(string userToDelete)
        {
            if (userToDelete == GlobalVarables.SITE_ADMIN_ACCOUNT)
            {
                return BadRequest();
            }
            else
            {
                var appUser = await _userManager.FindByNameAsync(userToDelete);

                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest();
            }
        }
    }
}
