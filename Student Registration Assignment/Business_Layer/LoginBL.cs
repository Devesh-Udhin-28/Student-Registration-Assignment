using Student_Registration_Assignment.Data_Access_Layer;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Business_Layer
{
    public class LoginBL
    {
        public static bool AuthenticateUser(LoginModel model)
        {
            return LoginDAL.AuthenticateUser(model);
        }

        public static LoginModel GetEmployeeDetailsWithRoles(LoginModel model)
        {
            return LoginDAL.GetEmployeeDetailsWithRoles(model);
        }
    }
}