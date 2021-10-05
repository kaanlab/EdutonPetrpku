using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Shared
{
    public class Page
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string InitialText { get; set; }
    }
}
