using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Debit
    {
        public int DebitID { get; set; }
        public string AdressID { get; set; }
        public string UserID { get; set; }
        public double debit { get; set; }
    }
}
