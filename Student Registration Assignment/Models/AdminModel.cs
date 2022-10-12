using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Models
{
    public class AdminModel
    {
        public int AdminIdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}