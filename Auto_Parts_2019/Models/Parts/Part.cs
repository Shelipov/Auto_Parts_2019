using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Part
    {
        [Key]
        public int ID { get; set; }
        public string Number { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Foto_link { get; set; }
        public string Group_Parts { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Analogues { get; set; }
        public string Group_Auto { get; set; }
    }
}
