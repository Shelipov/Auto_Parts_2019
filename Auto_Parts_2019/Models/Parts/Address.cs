using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Address :IdentityUser
    {
        //[Key]
        public string AddressID { get; set; }
        public string Country { get; set; }
        public string Sity { get; set; }
        public string Avenue { get; set; }
        public int Index { get; set; }
        public string IP { get; set; }
        public int Discount { get; set; }
        public IdentityUser UserID { get; set; }


    }
}
