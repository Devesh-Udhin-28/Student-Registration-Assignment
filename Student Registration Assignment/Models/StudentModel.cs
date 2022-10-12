using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Models
{
    public class StudentModel
    {
        public int StudentIdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string GuardianName { get; set; }
        public string DateOfBirth { get; set; }
        public int TotalPoints { get; set; }
        public string Status { get; set; }

    }
}