using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public class Nationality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public int Population { get; set; }

        // relationship
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }


        public NationalityViewModel ToNationalityViewModel() => new NationalityViewModel()
        {
            Id = this.Id,
            Name = this.Name,
            Population = this.Population,
            Subject = this.Subject,
            AppUserId = this.AppUserId,
            AppUserDisplayName = this.AppUser?.DisplayName ?? "свободно"
        };
            
    }
}
