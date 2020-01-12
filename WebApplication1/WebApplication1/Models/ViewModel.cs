using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Dal;

namespace WebApplication1.Models
{
    public class ViewModel
    {
        public Courses cour { get; set; }
        public StudentsCourses dat { get; set; }
    }
}