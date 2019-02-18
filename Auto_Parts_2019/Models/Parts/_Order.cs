using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class _Order
    { 
        public int ID { get; set; }
        public string SessionID { get; set; }
        public string OrderID { get; set; }
        public int PartID { get; set; }
        public string Number { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Group_Parts { get; set; }
        public int Quantity { get; set; }
        public string AddressID { get; set; }
        public string Country { get; set; }
        public string Sity { get; set; }
        public string Avenue { get; set; }
        public string IP { get; set; }
        public string Comment { get; set; }
        public bool OneCCreate { get; set; }
        public string UserID { get; set; }
    }
}
