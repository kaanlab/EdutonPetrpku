using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public class LoginResultViewModel
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
