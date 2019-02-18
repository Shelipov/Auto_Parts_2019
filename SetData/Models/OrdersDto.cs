using System;
using System.Collections.Generic;

namespace SetData
{
    public partial class OrdersDto
    {
        public string OrderId { get; set; }
        public int PartId { get; set; }
        public string Number { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string FotoLink { get; set; }
        public string GroupParts { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Analogues { get; set; }
        public string GroupAuto { get; set; }
        public string AddressId { get; set; }
        public string Country { get; set; }
        public string Sity { get; set; }
        public string Avenue { get; set; }
        public string Ip { get; set; }
        public string Comment { get; set; }
    }
}
