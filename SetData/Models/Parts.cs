using System;
using System.Collections.Generic;

namespace SetData
{
    public partial class Parts
    {
        public Parts()
        {
            CartLine = new HashSet<CartLine>();
            Courses = new HashSet<Courses>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Number { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string FotoLink { get; set; }
        public string GroupParts { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Analogues { get; set; }
        public string GroupAuto { get; set; }
        public bool OneCcreate { get; set; }

        public ICollection<CartLine> CartLine { get; set; }
        public ICollection<Courses> Courses { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
