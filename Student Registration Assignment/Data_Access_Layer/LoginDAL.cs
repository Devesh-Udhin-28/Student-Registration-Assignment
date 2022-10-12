using Microsoft.AspNet.Identity;
using Student_Registration_Assignment.Data_Access_Layer.Common;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Data_Access_Layer
{
    public class LoginDAL
    {

        public const string AuthenticateUserQueryStudent = @"SELECT stud.* FROM Student stud with(nolock)
                                                      INNER JOIN Login_User lu on stud.NID = lu.NID
                                                      WHERE stud.EmailAddress = @EmailAddress
                                                      AND lu.Password = @Password";

        public const string AuthenticateUserQueryAdmin = @"SELECT ad.* FROM Admin ad with(nolock)
                                                      INNER JOIN Login_User lu on ad.NID = lu.NID
                                                      WHERE ad.EmailAddress = @EmailAddress
                                                      AND lu.Password = @Password";

        public const string RetrievingPasswordStudent = @"SELECT Password
                                                   FROM Login_User lu
                                                   INNER JOIN Student stud on stud.NID = lu.NID
                                                   WHERE stud.EmailAddress = @EmailAddress";

        public const string RetrievingPasswordAdmin = @"SELECT Password
                                                   FROM Login_User lu
                                                   INNER JOIN Admin ad on ad.NID = lu.NID
                                                   WHERE ad.EmailAddress = @EmailAddress";


        public const string GetUserByEmailAddressStudent = @"SELECT stud.*,r.*
                                                             FROM [dbo].[Student] stud WITH(nolock)
                                                             INNER JOIN Login_User lu ON stud.NID = lu.NID
                                                             INNER JOIN Role r ON lu.RoleId = r.RoleId
                                                             WHERE stud.EmailAddress = @EmailAddress";

        public const string GetUserByEmailAddressAdmin = @"SELECT ad.*,r.*
                                                             FROM [dbo].[Admin] ad
                                                             INNER JOIN Login_User lu ON ad.NID = lu.NID
                                                             INNER JOIN Role r ON lu.RoleId = r.RoleId
                                                             WHERE ad.EmailAddress = @EmailAddress";


        public static bool AuthenticateUser(LoginModel model)
        {

            string passwd = "";

            PasswordHasher VerifyPassword = new PasswordHasher();

            List<SqlParameter> RetrievingPasswordParameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", model.EmailAddress),
            };

            // verifying if password exist in table
            var PasswordDT = DatabaseCommand.GetDataWithConditions(RetrievingPasswordStudent, RetrievingPasswordParameters);

            if (PasswordDT.Rows.Count == 0)
            {
                PasswordDT = DatabaseCommand.GetDataWithConditions(RetrievingPasswordAdmin, RetrievingPasswordParameters);
            }

            foreach (DataRow row in PasswordDT.Rows)
            {
                var DoesPasswordExist = VerifyPassword.VerifyHashedPassword(row["Password"].ToString(), model.Password);

                if (DoesPasswordExist == PasswordVerificationResult.Success)
                {
                    passwd = row["Password"].ToString();
                    break;
                }
            }

            // if password exist, then "passwd" will have its correct value else it will be empty and query will fail
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", model.EmailAddress),
                new SqlParameter("@Password", passwd)
            };

            var dt = DatabaseCommand.GetDataWithConditions(AuthenticateUserQueryStudent, parameters);

            if (dt.Rows.Count == 0)
            {
                dt = DatabaseCommand.GetDataWithConditions(AuthenticateUserQueryAdmin, parameters);
            }

            return dt.Rows.Count > 0;
        }

        public static LoginModel GetEmployeeDetailsWithRoles(LoginModel model)
        {
            LoginModel user = new LoginModel();

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", model.EmailAddress)
            };

            var dt = DatabaseCommand.GetDataWithConditions(GetUserByEmailAddressStudent, parameters);

            if(dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    user.NID = row["NID"].ToString();
                    user.RoleName = row["RoleName"].ToString();
                    user.EmailAddress = row["EmailAddress"].ToString().Trim();
                    user.RoleId = int.Parse(row["RoleId"].ToString());
                    user.Student_Status = row["Status"].ToString();
                }
            }
            // if email is not found in students table we search in Admin table
            else if(dt.Rows.Count == 0)
            {
                dt = DatabaseCommand.GetDataWithConditions(GetUserByEmailAddressAdmin, parameters);

                foreach (DataRow row in dt.Rows)
                {
                    user.NID = row["NID"].ToString();
                    user.RoleName = row["RoleName"].ToString();
                    user.EmailAddress = row["EmailAddress"].ToString().Trim();
                    user.RoleId = int.Parse(row["RoleId"].ToString());
                }
            }

            return user;
        }

    }
}