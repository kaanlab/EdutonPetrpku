using EdutonPetrpku.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginViewModel loginModel);
        Task Logout();
    }
}
