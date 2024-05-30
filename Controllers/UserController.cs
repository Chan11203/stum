using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StuMSystem.Models;

namespace StuMSystem.Controllers
{
    public class UserController : Controller
    {
        private StuMEntities db = new StuMEntities();

        // GET: users
        public ActionResult Index()
        {
            var users = db.users.ToList();
            return View(users);
        }

        private user GetUserById(int id)
        {
            return db.users.Find(id);
        }


        // GET: students/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.users, "id", "userName");
            return View();
        }

        public user Create([Bind(Include = "userName,password,role,students")] user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return user;
            }
            throw new InvalidOperationException("Invalid ModelState");
        }


        public void UpdateUser(user user)
        {
            var userToUpdate = db.users.Find(user.id);
            if (userToUpdate != null)
            {
                userToUpdate.userName = user.userName;
                userToUpdate.password = user.password;
                userToUpdate.role = user.role;

                db.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}