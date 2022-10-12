using Student_Registration_Assignment.Data_Access_Layer.Common;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Data_Access_Layer
{
    public class AdminDAL
    {
        public const string GetAdminsQuery = @"SELECT [NID],
                                                        [Name],
                                                        [Surname],
                                                        [Address],
                                                        [PhoneNumber],
                                                        [DateOfBirth],
                                                        [EmailAddress]
                                               FROM [dbo].[Admin]";

        public static List<AdminModel> GetAdmins()
        {
            List<AdminModel> admins = new List<AdminModel>();

            AdminModel admin;

            var dt = DatabaseCommand.GetData(GetAdminsQuery);

            foreach (DataRow row in dt.Rows)
            {
                admin = new AdminModel();
                admin.AdminIdentityNumber = int.Parse(row["NID"].ToString());
                admin.Name = row["Name"].ToString();
                admin.Surname = row["Surname"].ToString();
                admin.Address = row["Address"].ToString();
                admin.PhoneNumber = row["PhoneNumber"].ToString();
                admin.DateOfBirth = row["DateOfBirth"].ToString();
                admin.EmailAddress = row["EmailAddress"].ToString();

                admins.Add(admin);
            }

            return admins;
        }
    }
}