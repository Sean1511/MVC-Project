using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Dal
{
    public class DataDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Data>().ToTable("StudentsCourses").HasKey(x => new { x.StudentID, x.CourseID });
        }

        public DbSet<Data> StudentsCourses { get; set; }
    }
}