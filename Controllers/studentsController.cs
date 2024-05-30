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
    public class studentsController : Controller
    {
        private StuMEntities db = new StuMEntities();
        private UserController userController = new UserController();

        // GET: students
        public ActionResult Index()
        {
            var students = db.students.Include(s => s.user).ToList();
            return View(students);
        }


        // GET: students/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.users, "id", "userName");
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "studentName,gender,email,address,tel,major,dateofbirth")] student student)
        {
            if (ModelState.IsValid)
            {
                user user = new user("st123", 2);
                user = userController.Create(user);
                student.userId = user.id;
                db.students.Add(student);
                db.SaveChanges();
                user.userName = student.studentId.ToString();
                userController.UpdateUser(user);
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.users, "id", "userName", student.userId);
            return View(student);
        }


        // GET: students/Search 
        public ActionResult Search(string searchString)
        {
            // Kiểm tra xâu tìm kiếm không rỗng 
            if (!String.IsNullOrEmpty(searchString))
            {
                // Tìm sinh viên theo tên hoặc mã số sinh viên 
                var students = db.students.Include(s => s.user)
                                           .Where(s => s.studentName.Contains(searchString) || s.studentId.ToString().Contains(searchString));
                return View(students.ToList());
            }
            else
            {
                // Nếu xâu tìm kiếm rỗng, hiển thị toàn bộ sinh viên 
                var students = db.students.Include(s => s.user);
                return View(students.ToList());
            }
        }
        // GET: Student/Update/5
        public ActionResult Update(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.FirstOrDefault(s => s.studentId == id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Update/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "studentId,userId,studentName,gender,email,address,tel,major,dateofbirth")] student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }


        private void DeleteStudent(int studentId)
        {
            var student = db.students.Find(studentId);
            if (student != null)
            {
                db.students.Remove(student);
                db.SaveChanges();
            }
        }

        // GET: students/Delete/5
        public ActionResult Delete(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            student student = db.students.Find(id);
            db.students.Remove(student);
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