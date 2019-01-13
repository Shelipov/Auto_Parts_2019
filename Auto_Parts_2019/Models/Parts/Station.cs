using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Station
    {
        public int Id { get; set; }
        public string PlaceName { get; set; } // название места
        
        public double GeoLong { get; set; } // долгота - для карт google
        public double GeoLat { get; set; } // широта - для карт google
    }
}
