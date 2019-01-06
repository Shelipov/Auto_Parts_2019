using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Course
    {
        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public double CourseEuro { get; set; }
        public double CourseDollar { get; set; }
        public DateTime DateLastModified { get; set; }
        public Part Part { get; set; }
    }
}
