using Microsoft.AspNet.Identity;
using Student_Registration_Assignment.Data_Access_Layer;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Business_Layer
{
    public class StudentBL
    {
        public static List<StudentModel> GetStudents()
        {
            return StudentDAL.GetStudents();
        }

        public static List<StudentModel> GetStudentsCSV(string condition)
        {
            return StudentDAL.GetStudentsCSV(condition);
        }
        
        public static List<StudentModel> GetStudentsWithConditions(string condition)
        {
            return StudentDAL.GetStudentsWithConditions(condition);
        }

        public static bool RegisterStudent(StudentModel student, LoginModel login)
        {

            PasswordHasher HashingPassword = new PasswordHasher();
            login.Password = HashingPassword.HashPassword(login.Password);

            return StudentDAL.RegisterStudent(student, login);
        }

        public static bool VerifyingExistingStudents(StudentModel student)
        {
            return StudentDAL.VerifyingExistingStudents(student);
        }

        public static bool VerifyingExistingIDStudents(StudentModel student)
        {
            return StudentDAL.VerifyingExistingIDStudents(student);
        }

        public static List<SubjectsModel> GetSubjects()
        {
            return StudentDAL.GetSubjects();
        }

        public static bool RegisterSubjects(SubjectsModel subject, Student_SubjectModel StudentSubject)
        {
            return StudentDAL.RegisterSubjects(subject, StudentSubject);
        }

        public static bool VerifyingExistingRegisteredSubjects(SubjectsModel subject, Student_SubjectModel StudentSubject)
        {
            return StudentDAL.VerifyingExistingRegisteredSubjects(subject, StudentSubject);
        }

        public static int GettingNumberRegisteredSubjects(Student_SubjectModel StudentSubject)
        {
            return StudentDAL.GettingNumberRegisteredSubjects(StudentSubject);
        }

    }
}