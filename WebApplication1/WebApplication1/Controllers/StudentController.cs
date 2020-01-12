using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Dal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Index()
        {
            UserDal Userdal = new UserDal();
            using (MVCEntities db = new MVCEntities())
            {
                List<Courses> details = db.Courses.ToList();
                List<StudentsCourses> grades = db.StudentsCourses.ToList();

                var studentRecord = from d in details
                                    join g in grades on d.CourseId equals g.CourseID into table1
                                    from g in table1.ToList()
                                    select new ViewModel
                                    {
                                        cour = d,
                                        dat = g,
                                    };
                string Username = Session["UserName"].ToString();
                ViewBag.ID = (from x in Userdal.Users
                              where x.UserName == Username
                              select x.ID).FirstOrDefault();
                ViewBag.noGrade = "Not examine yet";
                return View("StudentHome", studentRecord);
            }
        }
        
        public ActionResult Logout()
        {
            Session["UserName"] = "";
            return RedirectToAction("HomePage", "Home");
        }
    }
}