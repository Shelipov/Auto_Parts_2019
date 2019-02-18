using System;
using System.Collections.Generic;

namespace SetData
{
    public partial class CartLine
    {
        public CartLine()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public int? PartId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateLastModified { get; set; }

        public Parts Part { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
