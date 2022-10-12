using Student_Registration_Assignment.Business_Layer;
using Student_Registration_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Registration_Assignment.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Authenticate(LoginModel model)
        {
            var IsUserValid = LoginBL.AuthenticateUser(model);

            if (IsUserValid)
            {
                LoginModel userInfo = LoginBL.GetEmployeeDetailsWithRoles(model);
                this.Session["CurrentUser"] = userInfo;
                this.Session["CurrentUserID"] = userInfo.NID;
                this.Session["CurrentRole"] = userInfo.RoleName;
                this.Session["CurrentStudentStatus"] = userInfo.Student_Status;

            }

            var choice = Session["selectedChoice"];

            if ((string)choice == "clickedOnApplyNow")
            {
                return Json(new { result = IsUserValid, url = Url.Action("Apply", "Portal") });
            }
            else
            {
                return Json(new { result = IsUserValid, url = Url.Action("Index", "Portal") });
            }
        }

        [HttpPost]
        public JsonResult RegisterStudent(StudentModel student, LoginModel login)
        {

            var DoesStudentIDExists = StudentBL.VerifyingExistingIDStudents(student);

            if (DoesStudentIDExists)
            {
                return Json(new { result = false, studentIDExists = DoesStudentIDExists });
            }

            var DoesStudentExists = StudentBL.VerifyingExistingStudents(student);

            if(DoesStudentExists)
            {
                return Json(new { result = false, studentExists = DoesStudentExists });
            }

            var IsRegSuccess = StudentBL.RegisterStudent(student, login);
            return Json(new { result = IsRegSuccess, url = Url.Action("Index", "Login") });
        }

    }
}