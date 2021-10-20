using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public class NationalityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public int Population { get; set; }

        public string AppUserId { get; set; }
        public string AppUserDisplayName { get; set; }
    }
}
