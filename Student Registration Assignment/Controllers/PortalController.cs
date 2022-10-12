using Student_Registration_Assignment.Business_Layer;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;
using Student_Registration_Assignment.Data_Access_Layer.Common;

namespace Student_Registration_Assignment.Controllers
{
    public class PortalController : Controller
    {
        // GET: Portal
        public ActionResult Index()
        {
            //return View();

            var loggedUser = Session["CurrentUser"] as LoginModel;

            if (loggedUser != null && loggedUser.RoleName.Equals("Admin"))
            {
                return RedirectToAction("ListStudents");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ListStudents()
        {
            var all = StudentBL.GetStudents();
            ViewBag.StudentsList = all;

            var approve = StudentBL.GetStudentsWithConditions("Approved");
            ViewBag.ApprovedStudentsList = approve;

            var waiting = StudentBL.GetStudentsWithConditions("Waiting");
            ViewBag.WaitingStudentsList = waiting;

            var reject = StudentBL.GetStudentsWithConditions("Rejected");
            ViewBag.RejectedStudentsList = reject;

            return View();
        }

        public ActionResult Apply()
        {
            var loggedUser = Session["CurrentUser"] as LoginModel;

            if (loggedUser == null)
            {
                this.Session["selectedChoice"] = "clickedOnApplyNow";

                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = StudentBL.GetSubjects();
                ViewBag.SubjectList = data;

                var loggedUserID = Session["CurrentUserID"];
                ViewBag.UserID = loggedUserID;

                return View();
            }
        }

        [HttpPost]
        public JsonResult RegisterSubjects(SubjectsModel subject, Student_SubjectModel StudentSubject)
        {
            var CanRegisterNumberOfSelectedSubjects = StudentBL.VerifyingExistingRegisteredSubjects(subject, StudentSubject);
            int NumberOfAlreadyRegisteredSubjects = StudentBL.GettingNumberRegisteredSubjects(StudentSubject);

            if (!CanRegisterNumberOfSelectedSubjects)
            {
                return Json(new { result = false, canRegisterSubjects = CanRegisterNumberOfSelectedSubjects, numOfSubjects = NumberOfAlreadyRegisteredSubjects });
            }

            var IsRegSuccess = StudentBL.RegisterSubjects(subject, StudentSubject);

            return Json(new { result = IsRegSuccess, url = Url.Action("Index", "Portal") });
        }

        public void ExtractCSV()
        {
            StringWriter strw = new StringWriter();
            strw.WriteLine("FirstName, LastName, TotalPoints, Status");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=All_Students_{0}.csv", DateTime.Now));

            Response.ContentType = "text/csv";

            var students = StudentBL.GetStudents();

            foreach (var student in students)
            {
                strw.WriteLine(string.Format("{0}, {1}, {2}, {3}", student.Name, student.Surname, student.TotalPoints, student.Status));
            }

            Response.Write(strw.ToString());
            Response.End();
        }

        public void ExtractConditionedCSV(string name)
        {
            StringWriter strw = new StringWriter();
            strw.WriteLine("FirstName, LastName, TotalPoints, Status");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=" + name + "_{0}.csv", DateTime.Now));

            Response.ContentType = "text/csv";

            var students = StudentBL.GetStudentsCSV(name);

            foreach (var student in students)
            {
                strw.WriteLine(string.Format("{0}, {1}, {2}, {3}", student.Name, student.Surname, student.TotalPoints, student.Status));
            }

            Response.Write(strw.ToString());
            Response.End();
        }
    }
}