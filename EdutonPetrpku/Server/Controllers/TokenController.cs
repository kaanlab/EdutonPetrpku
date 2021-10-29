using EdutonPetrpku.Server.Services;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Authorization;
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
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;
        public TokenController(IJwtService jwtService, UserManager<AppUser> userManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenViewModel>> Refresh(TokenViewModel tokenViewModel)
        {
            if (tokenViewModel is null)
            {
                return BadRequest(new RefreshTokenViewModel { Successful = false });
            }
            string accessToken = tokenViewModel.Token;
            string refreshToken = tokenViewModel.RefreshToken;
            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken, GlobalVarables.KEY);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest(new RefreshTokenViewModel { Successful = false });
            }

            var newAccessToken = await _jwtService.CreateToken(user, GlobalVarables.KEY);

            return Ok(new RefreshTokenViewModel { Token = newAccessToken, Successful = true });
        }


        [HttpPost("revoke"), Authorize]
        public async Task<ActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

    }
}
