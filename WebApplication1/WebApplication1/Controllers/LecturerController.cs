using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Dal;


namespace WebApplication1.Controllers
{
    public class LecturerController : Controller
    {
        UserDal Userdal = new UserDal();
        CourseDal Coursedal = new CourseDal();
        DataDal dataDal = new DataDal();

        public ActionResult Index()
        {
            string Username = Session["UserName"].ToString();
            string Fname = (from x in Userdal.Users
                            where x.UserName == Username
                            select x.FirstName).FirstOrDefault();
            string Lname = (from x in Userdal.Users
                            where x.UserName == Username
                            select x.LastName).FirstOrDefault();
            string FullName = Fname + " " + Lname;
            List<Course> courses = (from x in Coursedal.Courses
                                    where x.LecturerName == FullName
                                    select x).ToList<Course>();
            ViewBag.Name = FullName;
            ViewBag.Courses = courses;
            return View("LecturerHome");
        }

        public ActionResult ViewStudents(string id)
        {
            List<Data> StudList = (from x in dataDal.StudentsCourses
                                   where x.CourseID == id
                                   select x).ToList<Data>();
            ViewBag.course = (from x in dataDal.StudentsCourses
                              where x.CourseID == id
                              select x.CourseName).FirstOrDefault();
            ViewBag.List = StudList;
            ViewBag.noGrade = "Not examine";
            return View();
        }

        public ActionResult UpdateGrades(string name, string id)
        {
            Course course = Coursedal.Courses.Where(x => x.CourseId == id).First();
            ViewBag.UpdateMoedA = 0;
            ViewBag.UpdateMoedB = 0;
            if (course.MoedA < DateTime.Now)
            {
                ViewBag.UpdateMoedA = 1;
            }
            if (course.MoedB < DateTime.Now)
            {
                ViewBag.UpdateMoedB = 1;
            }
            ViewBag.noUpdate = "Exam has not yet taken place";
            Data lst = dataDal.StudentsCourses.Where(x => x.StudentName == name && x.CourseID == id).FirstOrDefault();
            return View(lst);
        }

        [HttpPost]
        public ActionResult SubmitGrade(Data dat)
        {
            if (ModelState.IsValid)
            {
                Data old = dataDal.StudentsCourses.Where(x => x.CourseID == dat.CourseID && x.StudentName == dat.StudentName).First();
                old.GradeMoedA = dat.GradeMoedA;
                old.GradeMoedB = dat.GradeMoedB;
                dataDal.SaveChanges();
                ViewBag.update = "Grades successfully updated";
                return RedirectToAction("Index");
            }
            else
                return View("ViewStudents", dat);
        }

        public ActionResult Logout()
        {
            Session["UserName"] = "";
            return RedirectToAction("HomePage", "Home");
        }
    }
}