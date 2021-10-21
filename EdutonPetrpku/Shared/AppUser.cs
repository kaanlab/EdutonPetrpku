using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        // relationship
        public Nationality Nationality { get; set; }

        //public List<Article> Articles { get; set; }


        public AppUserViewModel ToAppUserViewModel() => new AppUserViewModel()
        {
            DisplayName = this.DisplayName,
            Image = this.Image,
            Email = this.Email,
            UserName = this.UserName
        };

    }
}
