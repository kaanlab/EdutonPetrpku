using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public static class GlobalVarables
    {
        public static readonly SymmetricSecurityKey KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sOmeran0meString!"));

        public static readonly string SITE_ADMIN_ACCOUNT = "siteadmin";
        public static class Roles
        {
            public const string ADMIN = "admin";
            public const string USER = "user";
        }
    }
}
