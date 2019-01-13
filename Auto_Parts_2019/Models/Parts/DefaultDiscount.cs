using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class DefaultDiscount
    {
        [Key]
        public int ID { get; set; }
        public int TheDefaultDiscount { get; set; }
    }
}
