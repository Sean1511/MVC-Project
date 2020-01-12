using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Dal;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Authenticate(User user)
        {
            UserDal dal = new UserDal();
            
            List<User> userValid = (from u in dal.Users
                                    where u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password)
                                    select u).ToList<User>();

            if (userValid.Count == 1)
            {
                if (userValid[0].Permission == 1)
                {
                    Session["UserName"] = userValid[0].UserName;
                    return RedirectToRoute("AdminHome");
                }
                else if(userValid[0].Permission == 2)
                {
                    Session["UserName"] = userValid[0].UserName;
                    return RedirectToRoute("LecturerHome");
                }
                else
                {
                    Session["UserName"] = userValid[0].UserName;
                    return RedirectToRoute("StudentHome");
                }
            }
            else
            {
                ViewBag.Message = "User does not exist";
                return View("Login", user);
            }
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserName"] != null)
                return View("LoggedIn");
            else
                return RedirectToAction("Login");
        }
    }
}