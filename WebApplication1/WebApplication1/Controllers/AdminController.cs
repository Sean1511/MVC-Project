using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Dal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        UserDal Userdal = new UserDal();
        CourseDal Coursedal = new CourseDal();
        DataDal dataDal = new DataDal();


        // Home action
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
            Session["AdminName"] = FullName;
            return View("AdminHome");
        }

     
        //Register action
        public ActionResult Register()
        {
            return View(new User());
        }


        //Check login
        [HttpPost]
        public ActionResult Submit(User user)
        {
            List<User> users = (from x in Userdal.Users
                                select x).ToList<User>();
            foreach (User us in users)
            {
                if (us.ID == user.ID)
                {
                    ViewBag.IdMessage = "User ID " + user.ID + " already exist!";
                    return View("Register", user);
                }
            }

            foreach (User us in users)
            {
                if (us.UserName == user.UserName)
                {
                    ViewBag.IdMessage = "User name " + user.UserName + " already exist!";
                    return View("Register", user);
                }
            }

            if (ModelState.IsValid)
            {
                Userdal.Users.Add(user);
                Userdal.SaveChanges();
                ViewBag.message = "Registration completed";
                return View("AdminHome");
            }
            else
                return View("Register", user);
        }


        //Add course action
        public ActionResult AddCourse()
        {
            return View(new Course());
        }



        //Check the new course
        [HttpPost]
        public ActionResult SubmitCourse(Course course)
        {
            List<Course> courses = (from x in Coursedal.Courses
                                    select x).ToList<Course>();
            if (course.MoedA >= course.MoedB)
            {
                ViewBag.messageError = "The Moed B exam can not take place before Moed A";
                return View("AddCourse", course);
            }

            if (course.MoedA.Year < 2020 || course.MoedA.Year > 2021)
            {
                ViewBag.messageError = "Can not except years before 2020 or above 2021 " + "-" + course.MoedA.Year;
                return View("AddCourse", course);
            }

            if (course.MoedB.Year < 2020 || course.MoedB.Year > 2021)
            {
                ViewBag.messageError = "Can not except years before 2020 or above 2021 " + "-" + course.MoedB.Year;
                return View("AddCourse", course);
            }

            foreach (Course cs in courses)
            {
                if (course.CourseId == cs.CourseId)
                {
                    ViewBag.messageError = "Course id" + course.CourseId + " already exist!";
                    return View("AddCourse", course);
                }

                if (course.CourseName == cs.CourseName && course.CourseId != cs.CourseId)
                {
                    if (course.MoedA != cs.MoedA || course.MoedB != cs.MoedB)
                    {
                        ViewBag.messageError = "The dates of exams in course " + course.CourseName + " " + course.CourseId + " must be equals to the dates of course " + cs.CourseName + " " + cs.CourseId;
                        return View("AddCourse", course);
                    }
                }

                if (course.MoedA.Date == cs.MoedA.Date && (course.MoedA.Hour >= cs.MoedA.Hour && course.MoedA.Hour <= cs.MoedA.Hour + 3) && course.ExamAClass == cs.ExamAClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamAClass + " is already occupied between " + cs.MoedA + " to " + cs.MoedA.AddHours(3);
                    return View("AddCourse", course);
                }

                if (course.MoedA.Date == cs.MoedB.Date && (course.MoedA.Hour >= cs.MoedB.Hour && course.MoedA.Hour <= cs.MoedB.Hour + 3) && course.ExamAClass == cs.ExamBClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamBClass + " is already occupied between " + cs.MoedB + " to " + cs.MoedB.AddHours(3);
                    return View("AddCourse", course);
                }

                if (course.MoedB.Date == cs.MoedA.Date && (course.MoedB.Hour >= cs.MoedA.Hour && course.MoedB.Hour <= cs.MoedA.Hour + 3) && course.ExamBClass == cs.ExamAClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamAClass + " is already occupied between " + cs.MoedA + " to " + cs.MoedA.AddHours(3);
                    return View("AddCourse", course);
                }

                if (course.MoedB.Date == cs.MoedB.Date && (course.MoedB.Hour >= cs.MoedB.Hour && course.MoedB.Hour <= cs.MoedB.Hour + 3) && course.ExamBClass == cs.ExamBClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamBClass + " is already occupied between " + cs.MoedB + " to " + cs.MoedB.AddHours(3);
                    return View("AddCourse", course);
                }

                if (TimeCheck(course) == false)
                {
                    ViewBag.messageError = "The start time " + course.LectureStart + " must be before end time " + course.LectureEnd;
                    return View("AddCourse", course);
                }

                if (CheckDuplicateClassroom(course, cs) == false)
                {
                    ViewBag.messageError = "The class " + cs.Classroom + " is already occupied between " + cs.LectureStart + " to " + cs.LectureEnd + " in " + cs.LectuerDay;
                    return View("AddCourse", course);
                }

                if (CheckDuplicateLecturer(course, cs) == false)
                {
                    ViewBag.messageError = cs.LecturerName + " is already teaching between " + cs.LectureStart + " to " + cs.LectureEnd + " in " + cs.LectuerDay;
                    return View("AddCourse", course);
                }
            }

            if (ModelState.IsValid)
            {
                Coursedal.Courses.Add(course);
                Coursedal.SaveChanges();
                ViewBag.message1 = "Course " + course.CourseName + " successfully added";
                return View("AdminHome");
            }
            else
                return View("AddCourse", course);
        }


        //Assign student to course action
        public ActionResult AssignStudent()
        {
            return View(new Data());
        }



        //Check and submit the student details 
        [HttpPost]
        public ActionResult SubmitAssignStudent(Data dat)
        {
            List<Data> studInCourse = (from x in dataDal.StudentsCourses
                                       select x).ToList<Data>();
            List<Course> courses = (from x in Coursedal.Courses
                                    select x).ToList<Course>();
            dat.CourseName = (from x in Coursedal.Courses
                              where x.CourseId == dat.CourseID
                              select x.CourseName).FirstOrDefault().ToString();
            string fname = (from x in Userdal.Users
                            where x.ID == dat.StudentID
                            select x.FirstName).FirstOrDefault().ToString();
            string lname = (from x in Userdal.Users
                            where x.ID == dat.StudentID
                            select x.LastName).FirstOrDefault().ToString();
            dat.StudentName = fname + " " + lname;
            dat.GradeMoedA = -1;
            dat.GradeMoedB = -1;
            if (studInCourse.Count > 0)
            {
                foreach (Data studList in studInCourse)
                {
                    if (dat.StudentID == studList.StudentID && dat.CourseID == studList.CourseID)
                    {
                        ViewBag.messageDupCourse = "Student " + dat.StudentName + " ID number " + dat.StudentID + " already assigned to course " + dat.CourseName;
                        return View("AssignStudent", dat);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                dataDal.StudentsCourses.Add(dat);
                dataDal.SaveChanges();
                ViewBag.messageAssign = "Student " + dat.StudentName + " ID number " + dat.StudentID + " successfully assigned to course " + dat.CourseName;
                return View("AdminHome");
            }
            else
                return View("AssignStudent", dat);
        }


        // Select action
        public ActionResult SelectCourseToEdit()
        {
            return View(new Course());
        }



        // Select the course to edit
        [HttpPost]
        public ActionResult EditCourse(Course course)
        {
            Course currCourse = Coursedal.Courses.Where(x => x.CourseId == course.CourseId).First();
            return View(currCourse);
        }



        // Check and submit the course to edit
        [HttpPost]
        public ActionResult SubmitEditCourse(Course course)
        {
            Course oldCourse = Coursedal.Courses.Where(x => x.CourseId == course.CourseId).First();
            List<Course> courses = (from x in Coursedal.Courses
                                    where x.CourseId != oldCourse.CourseId
                                    select x).ToList<Course>();

            if (course.MoedA >= course.MoedB)
            {
                ViewBag.messageError = "The Moed B exam can not take place before Moed A";
                return View("EditCourse", course);
            }

            if (course.MoedA.Year < 2020 || course.MoedA.Year > 2021)
            {
                ViewBag.messageError = "Can not except years before 2020 or above 2021 " + "-" + course.MoedA.Year;
                return View("EditCourse", course);
            }

            if (course.MoedB.Year < 2020 || course.MoedB.Year > 2021)
            {
                ViewBag.messageError = "Can not except years before 2020 or above 2021 " + "-" + course.MoedB.Year;
                return View("EditCourse", course);
            }

            foreach (Course cs in courses)
            {

                if (course.CourseName == cs.CourseName && course.CourseId != cs.CourseId)
                {
                    if (course.MoedA != cs.MoedA || course.MoedB != cs.MoedB)
                    {
                        ViewBag.messageError = "The dates of exams in course " + course.CourseName + " " + course.CourseId + " must be equals to the dates of course " + cs.CourseName + " " + cs.CourseId;
                        return View("EditCourse", course);
                    }
                }

                if (course.MoedA.Date == cs.MoedA.Date && (course.MoedA.Hour >= cs.MoedA.Hour && course.MoedA.Hour <= cs.MoedA.Hour + 3) && course.ExamAClass == cs.ExamAClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamAClass + " is already occupied between " + cs.MoedA + " to " + cs.MoedA.AddHours(3);
                    return View("EditCourse", course);
                }

                if (course.MoedA.Date == cs.MoedB.Date && (course.MoedA.Hour >= cs.MoedB.Hour && course.MoedA.Hour <= cs.MoedB.Hour + 3) && course.ExamAClass == cs.ExamBClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamBClass + " is already occupied between " + cs.MoedB + " to " + cs.MoedB.AddHours(3);
                    return View("EditCourse", course);
                }

                if (course.MoedB.Date == cs.MoedA.Date && (course.MoedB.Hour >= cs.MoedA.Hour && course.MoedB.Hour <= cs.MoedA.Hour + 3) && course.ExamBClass == cs.ExamAClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamAClass + " is already occupied between " + cs.MoedA + " to " + cs.MoedA.AddHours(3);
                    return View("EditCourse", course);
                }

                if (course.MoedB.Date == cs.MoedB.Date && (course.MoedB.Hour >= cs.MoedB.Hour && course.MoedB.Hour <= cs.MoedB.Hour + 3) && course.ExamBClass == cs.ExamBClass)
                {
                    ViewBag.messageError = "The class " + cs.ExamBClass + " is already occupied between " + cs.MoedB + " to " + cs.MoedB.AddHours(3);
                    return View("EditCourse", course);
                }

                if (TimeCheck(course) == false)
                {
                    ViewBag.messageError = "The start time " + course.LectureStart + " must be before end time " + course.LectureEnd;
                    return View("EditCourse", course);
                }

                if (CheckDuplicateClassroom(course, cs) == false)
                {
                    ViewBag.messageError = "The class " + cs.Classroom + " is already occupied between " + cs.LectureStart + " to " + cs.LectureEnd + " in " + cs.LectuerDay;
                    return View("EditCourse", course);
                }

                if (CheckDuplicateLecturer(course, cs) == false)
                {
                    ViewBag.messageError = cs.LecturerName + " is already teaching between " + cs.LectureStart + " to " + cs.LectureEnd + " in " + cs.LectuerDay;
                    return View("EditCourse", course);
                }
            }

            if (ModelState.IsValid)
            {
                Coursedal.Courses.Remove(oldCourse);
                Coursedal.SaveChanges();
                Coursedal.Courses.Add(course);
                Coursedal.SaveChanges();
                ViewBag.edit = "Course " + course.CourseName + " successfully edited";
                return View("AdminHome");
            }
            else
                return View("EditCourse", course);
        }



        // Select course the update grades
        public ActionResult SelectCourseToUpdateGrades()
        {
            return View(new Course());
        }


        // Select student to update his grade
        [HttpPost]
        public ActionResult UpdateGrade(Course course)
        {
            List<Data> studInCourse = (from x in dataDal.StudentsCourses
                                       select x).ToList<Data>();
                        ViewBag.courseName = course.CourseName;
            ViewBag.students = studInCourse;
            ViewBag.noGrade = "Not examine";
            return View();
        }


        // Enter new grades page
        public ActionResult UpdateGradePage(string StudID, string CourseID)
        {
            Course course = Coursedal.Courses.Where(x => x.CourseId == CourseID).First();
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
            Data lst = dataDal.StudentsCourses.Where(x => x.StudentID == StudID && x.CourseID == CourseID).FirstOrDefault();
            return View(lst);
        }


        // Submit new grades
        [HttpPost]
        public ActionResult SubmitGrade(Data dat)
        {
            if (ModelState.IsValid)
            {
                Data old = dataDal.StudentsCourses.Where(x => x.CourseID == dat.CourseID && x.StudentID == dat.StudentID).First();
                old.GradeMoedA = dat.GradeMoedA;
                old.GradeMoedB = dat.GradeMoedB;
                dataDal.SaveChanges();
                ViewBag.update = "Grades successfully updated";
                return View("AdminHome");
            }
            else
                return View("UpdateGradePage", dat);
        }



        // Logout action
        public ActionResult Logout()
        {
            Session["UserName"] = "";
            return RedirectToAction("HomePage", "Home");
        }



        //Users table
        public ActionResult UsersTable()
        {
            List<User> users = (from x in Userdal.Users
                                select x).ToList<User>();
            return View(users);
        }


        //Users table
        public ActionResult CoursesTable()
        {
            List<Course> courses = (from x in Coursedal.Courses
                                    select x).ToList<Course>();
            return View(courses);
        }


        //Students courses table
        public ActionResult SCTable()
        {
            List<Data> SC = (from x in dataDal.StudentsCourses
                             select x).ToList<Data>();
            return View(SC);
        }


        //help functions
        public bool CheckDuplicateTime(Course NewCourse, Course ExistCourse)
        {
            if (ExistCourse.LectuerDay == NewCourse.LectuerDay)
            {
                StringBuilder NewLectureStart = new StringBuilder(NewCourse.LectureStart);
                StringBuilder NewLectureEnd = new StringBuilder(NewCourse.LectureEnd);
                StringBuilder LectureStart = new StringBuilder(ExistCourse.LectureStart);
                StringBuilder LectureEnd = new StringBuilder(ExistCourse.LectureEnd);
                NewLectureStart.Remove(2, 1);
                NewLectureEnd.Remove(2, 1);
                LectureStart.Remove(2, 1);
                LectureEnd.Remove(2, 1);
                int NewLectureStartHour = Int32.Parse(NewLectureStart.ToString());
                int NewLectureEndHour = Int32.Parse(NewLectureEnd.ToString());
                int LectureStartHour = Int32.Parse(LectureStart.ToString());
                int LectureEndHour = Int32.Parse(LectureEnd.ToString());
                
                if (NewLectureStartHour >= LectureStartHour && NewLectureStartHour <= LectureEndHour)
                    return false;
                if (NewLectureEndHour > LectureStartHour && NewLectureEndHour < LectureEndHour)
                    return false;
                if (LectureStartHour > NewLectureStartHour && LectureStartHour < NewLectureEndHour)
                    return false;
                if (LectureEndHour > NewLectureStartHour && LectureEndHour < NewLectureEndHour)
                    return false;
            }
            return true;
        }

        public bool TimeCheck(Course NewCourse)
        {
            StringBuilder NewLectureStart = new StringBuilder(NewCourse.LectureStart);
            StringBuilder NewLectureEnd = new StringBuilder(NewCourse.LectureEnd);
            NewLectureStart.Remove(2, 1);
            NewLectureEnd.Remove(2, 1);
            int NewLectureStartHour = Int32.Parse(NewLectureStart.ToString());
            int NewLectureEndHour = Int32.Parse(NewLectureEnd.ToString());
            if (NewLectureStartHour >= NewLectureEndHour)
                return false;
            return true;
        }

        public bool CheckDuplicateLecturer(Course NewCourse, Course ExistCourse)
        {
            if (ExistCourse.LecturerName == NewCourse.LecturerName)
            {
                if (CheckDuplicateTime(ExistCourse, NewCourse) == false)
                    return false;
            }
            return true;
        }

        public bool CheckDuplicateClassroom(Course NewCourse, Course ExistCourse)
        {
            if (ExistCourse.Classroom == NewCourse.Classroom)
            {
                if (CheckDuplicateTime(ExistCourse, NewCourse) == false)
                    return false;
            }
            return true;
        }
    }
}