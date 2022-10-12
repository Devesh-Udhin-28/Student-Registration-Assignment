using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Models
{
    public class SubjectsModel
    {
        public int SubID { get; set; }
        public string Name { get; set; }
        public int[] SubjectArray { get; set; }
    }
}