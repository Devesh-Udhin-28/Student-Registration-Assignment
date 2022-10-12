using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Models
{
    public class Student_SubjectModel
    {
        public int SubjectID { get; set; }
        public int StudentIdentityNumber { get; set; }
        public int[] PointsArray { get; set; }
    }
}