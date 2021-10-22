using EdutonPetrpku.Server.Services;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResultViewModel>> Login(LoginViewModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(loginModel.Login);
            if (user == null)
            {
                return Unauthorized(new LoginResultViewModel { Successful = false, Error = "Неверное имя пользователя или пароль!" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (result.Succeeded)
            {
                var refreshtoken = _jwtService.GenerateRefreshToken();
                user.RefreshToken = refreshtoken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _userManager.UpdateAsync(user);

                return Ok(
                    new LoginResultViewModel
                    {
                        Successful = true,
                        Token = await _jwtService.CreateToken(user, GlobalVarables.KEY),
                        RefreshToken = refreshtoken
                    });
            }

            return Unauthorized(new LoginResultViewModel { Successful = false, Error = "Неверное имя пользователя или пароль!" });
        }
    }
}
