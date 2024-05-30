using StuMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StuMSystem.Controllers
{
    public class LoginController : Controller
    {
        private StuMEntities db = new StuMEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(user users)
        {
            if (ModelState.IsValid)
            {
                var user = GetUserByUserNameAndPassword(users.userName, users.password);

                if (user != null)
                {
                    // Redirect based on user role
                    if (user.role == 1) // Admin
                    {
                        return RedirectToAction("_HomePage", "Admin");
                    }
                    else if (user.role ==2) // Student
                    {
                        return RedirectToAction("HomePage", "Regist");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If ModelState is invalid or user is not found
            return View(users);
        }


        //verify user's information in the database
        private user GetUserByUserNameAndPassword(string username, string password)
        {
            return db.users.FirstOrDefault(u => u.userName == username && u.password == password);
        }


        /*[HttpPost]
        public ActionResult Login(string username, string password)
        {
            Session["StudentId"] = "19034002";
            return RedirectToAction("HomePage", "Regist");
        }*/
    }
}