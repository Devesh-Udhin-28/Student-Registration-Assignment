using Student_Registration_Assignment.Data_Access_Layer.Common;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using Student_Registration_Assignment.Business_Layer;
using Newtonsoft.Json.Linq;

namespace Student_Registration_Assignment.Data_Access_Layer
{
    public class StudentDAL
    {
        public const string GetStudentsQuery = @"SELECT [NID],
                                                        [Name],
                                                        [Surname],
                                                        [Age],
                                                        [Address],
                                                        [PhoneNumber],
                                                        [DateOfBirth],
                                                        [GuardianName],
                                                        [EmailAddress],
                                                        [Country],
                                                        [Status],
                                                        [TotalPoints]
                                                 FROM [dbo].[Student]";

        public const string GetStudentsQueryCSV = @"SELECT TOP(15) *
                                                    FROM [dbo].[Student]
                                                    WHERE Status = @Status
                                                    ORDER BY TotalPoints";

        public const string GetStudentsQueryWithCondition = @"SELECT [NID],
                                                                     [Name],
                                                                     [Surname],
                                                                     [Age],
                                                                     [Address],
                                                                     [PhoneNumber],
                                                                     [DateOfBirth],
                                                                     [GuardianName],
                                                                     [EmailAddress],
                                                                     [Country]
                                                              FROM [dbo].[Student]
                                                              WHERE Status = @status";

        public const string RegisterStudentQuery_StudentTable = @"INSERT INTO [dbo].[Student] ([NID],
                                                                                               [Name],
                                                                                               [Surname],
                                                                                               [Address],
                                                                                               [PhoneNumber],
                                                                                               [DateOfBirth],
                                                                                               [GuardianName],
                                                                                               [EmailAddress],
                                                                                               [Age],
                                                                                               [Country])
                                                                   VALUES (@NID,
                                                                           @Name,
                                                                           @Surname,
                                                                           @Address,
                                                                           @PhoneNumber,
                                                                           @DateOfBirth,
                                                                           @GuardianName,
                                                                           @EmailAddress,
                                                                           @Age,
                                                                           @Country)";

        public const string RegisterStudentQuery_LoginTable = @"INSERT INTO [dbo].[Login_User] ([NID],
                                                                                                [Password],
                                                                                                [RoleID])
                                                                VALUES (@NID,
                                                                        @Password,
                                                                        @RoleID)";

        public const string VerifyingExistingStudentQuery = @"SELECT [EmailAddress]
                                                              FROM [dbo].[Student]
                                                              WHERE EmailAddress = @EmailAddress
                                                              OR EmailAddress IN (SELECT [EmailAddress]
                                                                                  FROM [dbo].[Login_User]
                                                                                  WHERE EmailAddress = @EmailAddress)";

        public const string VerifyingExistingIDStudentQuery = @"SELECT [NID]
                                                              FROM [dbo].[Student]
                                                              WHERE NID = @NID
                                                              OR NID IN (SELECT [NID]
                                                                         FROM [dbo].[Login_User]
                                                                         WHERE NID = @NID)";

        public const string GetSubjectsQuery = @"SELECT [SubID],
                                                        [Name]
                                                 FROM [dbo].[Subjects]";

        public const string RegisterSubjectQuery = @"INSERT INTO [dbo].[Students_Subjects] ([NID],
                                                                                            [SubID],
                                                                                            [Points])
                                                      VALUES (@NID,
                                                              @SubID,
                                                              @Points)";

        public const string CheckingAlreadyRegisteredSubjectQuery = @"SELECT COUNT(NID) AS Count
                                                                      FROM [dbo].[Students_Subjects]
                                                                      WHERE NID = @NID";

        public const string UpdateTotalPointsQuery = @"UPDATE [dbo].[Student]
                                                            SET [TotalPoints] = (SELECT SUM(Points)
						                                                         FROM Students_Subjects
						                                                         WHERE NID = @NID)
                                                            WHERE [NID] = @NID";

        public const string UpdateStatusQuery = @"UPDATE [dbo].[Student]
                                                            SET [Status] = @status
                                                            WHERE [NID] = @NID";

        public const string GetTotalPointsQuery = @"SELECT TotalPoints
                                                    FROM [dbo].[Student]
                                                    WHERE [NID] = @NID";

        public const string GetTotalSpecifiedStatusQuery = @"SELECT COUNT(*) AS Count
                                                             FROM [dbo].[Student]
                                                             WHERE [Status] = @status";

        public static List<StudentModel> GetStudents()
        {
            List<StudentModel> students = new List<StudentModel>();

            StudentModel student;

            var dt = DatabaseCommand.GetData(GetStudentsQuery);

            foreach(DataRow row in dt.Rows)
            {
                student = new StudentModel();

                student.StudentIdentityNumber = int.Parse(row["NID"].ToString()); 
                student.Name = row["Name"].ToString();
                student.Surname = row["Surname"].ToString();
                student.Age = int.Parse(row["Age"].ToString());
                student.Address = row["Address"].ToString();
                student.PhoneNumber = row["PhoneNumber"].ToString();
                student.DateOfBirth = row["DateOfBirth"].ToString();
                student.GuardianName = row["GuardianName"].ToString();
                student.EmailAddress = row["EmailAddress"].ToString();
                student.Country = row["Country"].ToString();
                student.Status = row["Status"].ToString();
                student.TotalPoints = int.Parse(row["TotalPoints"].ToString());

                students.Add(student);
            }

            return students;
        }

        public static List<StudentModel> GetStudentsCSV(string condition)
        {
            List<StudentModel> students = new List<StudentModel>();

            StudentModel student;

            List<SqlParameter> Parameter = new List<SqlParameter>
            {
                new SqlParameter("@status", condition),
            };

            var dt = DatabaseCommand.GetDataWithConditions(GetStudentsQueryCSV, Parameter);

            foreach (DataRow row in dt.Rows)
            {
                student = new StudentModel();

                student.StudentIdentityNumber = int.Parse(row["NID"].ToString());
                student.Name = row["Name"].ToString();
                student.Surname = row["Surname"].ToString();
                student.Age = int.Parse(row["Age"].ToString());
                student.Address = row["Address"].ToString();
                student.PhoneNumber = row["PhoneNumber"].ToString();
                student.DateOfBirth = row["DateOfBirth"].ToString();
                student.GuardianName = row["GuardianName"].ToString();
                student.EmailAddress = row["EmailAddress"].ToString();
                student.Country = row["Country"].ToString();
                student.Status = row["Status"].ToString();
                student.TotalPoints = int.Parse(row["TotalPoints"].ToString());

                students.Add(student);
            }

            return students;
        }

        public static List<StudentModel> GetStudentsWithConditions(string condition)
        {
            List<StudentModel> students = new List<StudentModel>();

            StudentModel student;

            List<SqlParameter> studentParameter = new List<SqlParameter>
            {
                new SqlParameter("@status", condition),
            };

            var dt = DatabaseCommand.GetDataWithConditions(GetStudentsQueryWithCondition, studentParameter);

            foreach (DataRow row in dt.Rows)
            {
                student = new StudentModel();

                student.StudentIdentityNumber = int.Parse(row["NID"].ToString());
                student.Name = row["Name"].ToString();
                student.Surname = row["Surname"].ToString();
                student.Age = int.Parse(row["Age"].ToString());
                student.Address = row["Address"].ToString();
                student.PhoneNumber = row["PhoneNumber"].ToString();
                student.DateOfBirth = row["DateOfBirth"].ToString();
                student.GuardianName = row["GuardianName"].ToString();
                student.EmailAddress = row["EmailAddress"].ToString();
                student.Country = row["Country"].ToString();

                students.Add(student);
            }

            return students;
        }

        public static bool RegisterStudent(StudentModel student, LoginModel login)
        {
            List<SqlParameter> RegisterStudentParameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", student.StudentIdentityNumber),
                new SqlParameter("@Name", student.Name),
                new SqlParameter("@Surname", student.Surname),
                new SqlParameter("@Address", student.Address),
                new SqlParameter("@PhoneNumber", student.PhoneNumber),
                new SqlParameter("@DateOfBirth", student.DateOfBirth),
                new SqlParameter("@GuardianName", student.GuardianName),
                new SqlParameter("@EmailAddress", student.EmailAddress),
                new SqlParameter("@Age", student.Age),
                new SqlParameter("@Country", student.Country)
            };

            List<SqlParameter> RegisterLoginParameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", student.StudentIdentityNumber),
                new SqlParameter("@Password", login.Password),
                new SqlParameter("@RoleID", 3),
            };

            return (DatabaseCommand.UpdateAndInsertData(RegisterStudentQuery_StudentTable, RegisterStudentParameter) & DatabaseCommand.UpdateAndInsertData(RegisterStudentQuery_LoginTable, RegisterLoginParameter)) > 0;
        }

        public static bool VerifyingExistingStudents(StudentModel student)
        {
            List<SqlParameter> CheckExistingStudentParameter = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", student.EmailAddress)
            };

            var dt = DatabaseCommand.GetDataWithConditions(VerifyingExistingStudentQuery, CheckExistingStudentParameter);

            return (dt.Rows.Count) > 0;
        }

        public static bool VerifyingExistingIDStudents(StudentModel student)
        {
            List<SqlParameter> CheckExistingStudentIDParameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", student.StudentIdentityNumber)
            };

            var dt = DatabaseCommand.GetDataWithConditions(VerifyingExistingIDStudentQuery, CheckExistingStudentIDParameter);

            return (dt.Rows.Count) > 0;
        }

        public static List<SubjectsModel> GetSubjects()
        {
            List<SubjectsModel> subjects = new List<SubjectsModel>();

            SubjectsModel subject;

            var dt = DatabaseCommand.GetData(GetSubjectsQuery);

            foreach (DataRow row in dt.Rows)
            {
                subject = new SubjectsModel();

                subject.SubID = int.Parse(row["SubID"].ToString());
                subject.Name = row["Name"].ToString();

                subjects.Add(subject);
            }

            return subjects;
        }

        public static bool RegisterSubjects(SubjectsModel subject, Student_SubjectModel StudentSubject)
        {

            List<SqlParameter> RegisterSubjectParameter;

            for(int i = 0; i < subject.SubjectArray.Length; i++)
            {
                RegisterSubjectParameter = new List<SqlParameter>
                {
                    new SqlParameter("@NID", StudentSubject.StudentIdentityNumber),
                    new SqlParameter("@SubID", subject.SubjectArray[i]),
                    new SqlParameter("@Points", StudentSubject.PointsArray[i])
                };

                if(DatabaseCommand.UpdateAndInsertData(RegisterSubjectQuery, RegisterSubjectParameter) > 0){
                    continue;
                }
                else
                {
                    return false;
                }
                
            }

            List<SqlParameter> UpdateTotalParameter = new()
            {
                new SqlParameter("@NID", StudentSubject.StudentIdentityNumber)
            };

            if( DatabaseCommand.UpdateAndInsertData(UpdateTotalPointsQuery, UpdateTotalParameter) > 0)
            {
                return UpdateStatus(StudentSubject.StudentIdentityNumber) > 0;
            }
            else
            {
                return false;
            }
        }

        public static bool VerifyingExistingRegisteredSubjects(SubjectsModel subject, Student_SubjectModel StudentSubject)
        {
            List<SqlParameter> CheckExistingRegisteredSubjectsParameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", StudentSubject.StudentIdentityNumber)
            };

            var dt = DatabaseCommand.GetDataWithConditions(CheckingAlreadyRegisteredSubjectQuery, CheckExistingRegisteredSubjectsParameter);

            int numberOfAlreadyRegisteredStudent = Convert.ToInt32(dt.Rows[0]["Count"]);

            if (numberOfAlreadyRegisteredStudent + subject.SubjectArray.Length > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int GettingNumberRegisteredSubjects(Student_SubjectModel StudentSubject)
        {
            List<SqlParameter> CheckExistingRegisteredSubjectsParameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", StudentSubject.StudentIdentityNumber)
            };

            var dt = DatabaseCommand.GetDataWithConditions(CheckingAlreadyRegisteredSubjectQuery, CheckExistingRegisteredSubjectsParameter);

            return Convert.ToInt32(dt.Rows[0]["Count"]);

        }

        public static int UpdateStatus(int nid)
        {

            List<SqlParameter> parameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", nid)
            };

            List<SqlParameter> StatusParameter = new List<SqlParameter>
            {
                new SqlParameter("@status", "Approved")
            };

            var dt = DatabaseCommand.GetDataWithConditions(GetTotalPointsQuery, parameter);

            var dtStatus = DatabaseCommand.GetDataWithConditions(GetTotalSpecifiedStatusQuery, StatusParameter);

            var totalPoints = dt.Rows[0]["TotalPoints"];
            int total_Points = Convert.ToInt32(totalPoints);

            var totalApprovedStudents = dtStatus.Rows[0]["Count"];
            int total_Approved_Students = Convert.ToInt32(totalApprovedStudents);

            string status = "";

            if(total_Points < 10)
            {
                status = "Rejected";
            }
            else if(total_Points >= 10 && total_Approved_Students < 15)
            {
                status = "Approved";
            }
            else
            {
                status = "Waiting";
            }

            List<SqlParameter> UpdateStatusParameter = new List<SqlParameter>
            {
                new SqlParameter("@NID", nid),
                new SqlParameter("@status", status)
            };

            return DatabaseCommand.UpdateAndInsertData(UpdateStatusQuery, UpdateStatusParameter);

        }
    }
}