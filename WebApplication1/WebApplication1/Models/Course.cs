using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Dal;

namespace WebApplication1.Models
{
    public class Course
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Course name must contain only letters and digits")]
        public string CourseName { get; set; }
        public SelectList GetAsCourseList()
        {
            CourseDal courseDal = new CourseDal();
            List<Course> courses = (from x in courseDal.Courses
                                    select x).ToList<Course>();
            var CourseName = from x in courses
                             select new
                             {
                                 course1 = x.CourseName,
                                 course2 = x.CourseName
                             };

            return new SelectList(CourseName, "course1", "course2");
        }

        public SelectList GetAsCourseIDList()
        {
            CourseDal courseDal = new CourseDal();
            List<Course> courses = (from x in courseDal.Courses
                                    select x).ToList<Course>();
            var CourseName = from x in courses
                             select new
                             {
                                 course1 = x.CourseId,
                                 course2 = x.CourseName +" - " + x.CourseId
                             };

            return new SelectList(CourseName, "course1", "course2");
        }

        [Required]
        public string LecturerName { get; set; }
        public SelectList GetAsSelectList()
        {
            UserDal Userdal = new UserDal();
            List<User> lectuerers = (from x in Userdal.Users
                                     where x.Permission == 2
                                     select x).ToList<User>();
            var FullName = from x in lectuerers
                           select new
                           {
                               FN = x.FirstName + " " + x.LastName,
                               LN = x.FirstName + " " + x.LastName
                           };

            return new SelectList(FullName, "FN", "LN");
        }
        [Key]
        [Required]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "Course ID must countain 3 digits exactly")]
        public string CourseId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode =true)]
        public DateTime MoedA { get; set; }

        public string ExamAClass { get; set; }
        public IEnumerable<SelectListItem> ExamAClassList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Safra100", Value = "Safra100"},
                    new SelectListItem { Text = "Safra101", Value = "Safra101"},
                    new SelectListItem { Text = "Safra102", Value = "Safra102"},
                    new SelectListItem { Text = "Shimon100", Value = "Shimon100"},
                    new SelectListItem { Text = "Shimon101", Value = "Shimon101"},
                    new SelectListItem { Text = "Shimon102", Value = "Shimon102"},
                    new SelectListItem { Text = "Einstein100", Value = "Einstein100"},
                    new SelectListItem { Text = "Einstein101", Value = "Einstein101"},
                    new SelectListItem { Text = "Einstein102", Value = "Einstein102"},
                    new SelectListItem { Text = "Katzir100", Value = "Katzir100"},
                    new SelectListItem { Text = "Katzir101", Value = "Katzir101"},
                    new SelectListItem { Text = "Katzir102", Value = "Katzir102"}
                };
            }
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime MoedB { get; set; }

        public string ExamBClass { get; set; }
        public IEnumerable<SelectListItem> ExamBClassList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Safra100", Value = "Safra100"},
                    new SelectListItem { Text = "Safra101", Value = "Safra101"},
                    new SelectListItem { Text = "Safra102", Value = "Safra102"},
                    new SelectListItem { Text = "Shimon100", Value = "Shimon100"},
                    new SelectListItem { Text = "Shimon101", Value = "Shimon101"},
                    new SelectListItem { Text = "Shimon102", Value = "Shimon102"},
                    new SelectListItem { Text = "Einstein100", Value = "Einstein100"},
                    new SelectListItem { Text = "Einstein101", Value = "Einstein101"},
                    new SelectListItem { Text = "Einstein102", Value = "Einstein102"},
                    new SelectListItem { Text = "Katzir100", Value = "Katzir100"},
                    new SelectListItem { Text = "Katzir101", Value = "Katzir101"},
                    new SelectListItem { Text = "Katzir102", Value = "Katzir102"}
                };
            }
        }

        [Required]
        public string LectuerDay { get; set; }
        public IEnumerable<SelectListItem> DayList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Sunday", Value = "Sunday"},
                    new SelectListItem { Text = "Monday", Value = "Monday"},
                    new SelectListItem { Text = "Tuesday", Value = "Tuesday"},
                    new SelectListItem { Text = "Wednesday", Value = "Wednesday"},
                    new SelectListItem { Text = "Thursday", Value = "Thursday"},
                    new SelectListItem { Text = "Friday", Value = "Friday"}
                };
            }
        }
        [Required]
        public string LectureStart { get; set; }
        public IEnumerable<SelectListItem> LectureStartList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "08:00", Value = "08:00"},
                    new SelectListItem { Text = "09:00", Value = "09:00"},
                    new SelectListItem { Text = "10:00", Value = "10:00"},
                    new SelectListItem { Text = "11:00", Value = "11:00"},
                    new SelectListItem { Text = "12:00", Value = "12:00"},
                    new SelectListItem { Text = "13:00", Value = "13:00"},
                    new SelectListItem { Text = "14:00", Value = "14:00"},
                    new SelectListItem { Text = "15:00", Value = "15:00"},
                    new SelectListItem { Text = "16:00", Value = "16:00"},
                    new SelectListItem { Text = "17:00", Value = "17:00"},
                    new SelectListItem { Text = "18:00", Value = "18:00"},
                    new SelectListItem { Text = "19:00", Value = "19:00"},
                    new SelectListItem { Text = "20:00", Value = "20:00"}
                };
            }
        }
        [Required]
        public string LectureEnd { get; set; }
        public IEnumerable<SelectListItem> LectureEndsList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "09:00", Value = "09:00"},
                    new SelectListItem { Text = "10:00", Value = "10:00"},
                    new SelectListItem { Text = "11:00", Value = "11:00"},
                    new SelectListItem { Text = "12:00", Value = "12:00"},
                    new SelectListItem { Text = "13:00", Value = "13:00"},
                    new SelectListItem { Text = "14:00", Value = "14:00"},
                    new SelectListItem { Text = "15:00", Value = "15:00"},
                    new SelectListItem { Text = "16:00", Value = "16:00"},
                    new SelectListItem { Text = "17:00", Value = "17:00"},
                    new SelectListItem { Text = "18:00", Value = "18:00"},
                    new SelectListItem { Text = "19:00", Value = "19:00"},
                    new SelectListItem { Text = "20:00", Value = "20:00"},
                    new SelectListItem { Text = "21:00", Value = "21:00"}
                };
            }
        }

        public string Classroom { get; set; }
        public IEnumerable<SelectListItem> ClassroomList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Safra100", Value = "Safra100"},
                    new SelectListItem { Text = "Safra101", Value = "Safra101"},
                    new SelectListItem { Text = "Safra102", Value = "Safra102"},
                    new SelectListItem { Text = "Shimon100", Value = "Shimon100"},
                    new SelectListItem { Text = "Shimon101", Value = "Shimon101"},
                    new SelectListItem { Text = "Shimon102", Value = "Shimon102"},
                    new SelectListItem { Text = "Einstein100", Value = "Einstein100"},
                    new SelectListItem { Text = "Einstein101", Value = "Einstein101"},
                    new SelectListItem { Text = "Einstein102", Value = "Einstein102"},
                    new SelectListItem { Text = "Katzir100", Value = "Katzir100"},
                    new SelectListItem { Text = "Katzir101", Value = "Katzir101"},
                    new SelectListItem { Text = "Katzir102", Value = "Katzir102"}
                };
            }
        }
    }
}