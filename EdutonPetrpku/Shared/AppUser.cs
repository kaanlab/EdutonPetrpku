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

        // relationship
        public Nationality Nationality { get; set; }

    }
}
