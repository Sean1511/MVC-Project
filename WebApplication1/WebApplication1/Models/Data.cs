using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Dal;


namespace WebApplication1.Models
{
    public class Data
    {
        public string StudentName { get; set; }
        
        [Key]
        public string StudentID { get; set; }
        public SelectList GetAsStudentsList()
        {
            UserDal Userdal = new UserDal();
            List<User> Students = (from x in Userdal.Users
                                   where x.Permission == 3
                                   select x).ToList<User>();
            var FullName = from x in Students
                           select new
                           {
                               FN = x.ID,
                               LN = x.FirstName + " " + x.LastName + " " + "-" + " " + x.ID
                           };

            return new SelectList(FullName, "FN", "LN");
        }

        public string CourseName { get; set; }
        
        [Key]
        public string CourseID { get; set; }
        public SelectList GetAsCourseList()
        {
            CourseDal courseDal = new CourseDal();
            List<Course> courses = (from x in courseDal.Courses
                                    select x).ToList<Course>();
            var CourseID = from x in courses
                             select new
                             {
                                 course1 = x.CourseId,
                                 course2 = x.CourseName + " " + "-" + " " + x.CourseId
                             };

            return new SelectList(CourseID, "course1", "course2");
        }

        [RegularExpression("^100|[1-9]?[0-9]|-1$", ErrorMessage = "The grade must be a number between 0-100 or -1 if not examine")]
        public int GradeMoedA { get; set; }

        [RegularExpression("^100|[1-9]?[0-9]|-1$", ErrorMessage = "The grade must be a number between 0-100 or -1 if not examine")]
        public int GradeMoedB { get; set; }
    }
}