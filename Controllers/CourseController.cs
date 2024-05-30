using StuMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using System.Data.Entity;

namespace StuMSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private StuMEntities db = new StuMEntities();

        // GET: Courses
        public ActionResult Index(string searchString)
        {
            var courses = from c in db.courses
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.courseName.Contains(searchString));
            }

            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "courseId,courseName,lecturerName,day,time,location")] course course)
        {
            if (ModelState.IsValid)
            {
                db.courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Update/5
        public ActionResult Update(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "courseId,courseName,lecturerName,day,time,location")] course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            course course = db.courses.Find(id);
            db.courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
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