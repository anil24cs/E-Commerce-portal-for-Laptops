using DigitalRetailersLaptop.Helpers;
using DigitalRetailersLaptop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalRetailersLaptop.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDBContext context;
        public AccountController()
        {
            context = new ApplicationDBContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Login(User user)
        {
            var userObj = context.Users.Where(u => u.Username == user.Username
            && u.Password == user.Password
            && u.UserType == user.UserType).SingleOrDefault();
            if (userObj != null)
            {
                SessionHelper.setObjectAsJson(HttpContext.Session, "user", userObj);
                User usr = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                if (usr.UserType == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Please Enter Valid Credentials.";
                return View("Index");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
