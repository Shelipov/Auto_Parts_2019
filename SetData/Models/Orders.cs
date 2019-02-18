using System;
using System.Collections.Generic;

namespace SetData
{
    public partial class Orders
    {
        public int OrderId { get; set; }
        public int? CartLineId { get; set; }
        public int? PartId { get; set; }
        public string AddressId { get; set; }
        public string Comment { get; set; }
        public bool OneCcreate { get; set; }

        public AspNetUsers Address { get; set; }
        public CartLine CartLine { get; set; }
        public Parts Part { get; set; }
    }
}
