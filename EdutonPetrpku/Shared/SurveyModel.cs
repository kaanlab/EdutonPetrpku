using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public class SurveyModel
    {
        [Required]
        public int NationalityId { get; set; }
        [Required]
        public string AppUserId { get; set; }
    }
}
