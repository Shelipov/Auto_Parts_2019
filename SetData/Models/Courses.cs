using System;
using System.Collections.Generic;

namespace SetData
{
    public partial class Courses
    {
        public int Id { get; set; }
        public double CourseEuro { get; set; }
        public double CourseDollar { get; set; }
        public DateTime DateLastModified { get; set; }
        public int? PartId { get; set; }

        public Parts Part { get; set; }
    }
}
