using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public CartLine CartLine { get; set; }
        public Part Part { get; set; }
        public Address Address { get; set; }
        public string Comment { get; set; }
    }
}
