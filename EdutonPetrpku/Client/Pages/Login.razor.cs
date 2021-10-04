using EdutonPetrpku.Client.Services;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Client.Pages
{
    public partial class Login
    {
        private LoginModel _loginModel = new LoginModel();

        [Inject]
        public IAuthService AuthService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool ShowAuthError { get; set; }
        public string Error { get; set; }
        public async Task ExecuteLogin()
        {
            ShowAuthError = false;
            var result = await AuthService.Login(_loginModel);
            if (!result.Successful)
            {
                Error = result.Error;
                ShowAuthError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
