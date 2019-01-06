using Auto_Parts_2019.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Helpers
{
    public class IndexViewModel
    {
        public IEnumerable<Part> Parts { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
