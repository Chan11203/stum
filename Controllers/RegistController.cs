using StuMSystem.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StuMSystem.Controllers
{
    public class RegistController : Controller
    {
        private StuMEntities db = new StuMEntities();
        // GET: Students
        public ActionResult HomePage()
        {
            return View();
        }
        public ActionResult RegistCourse()
        {
            //if (Session["StudentId"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            var courses = db.courses.ToList();

            return View(courses);
        }
        // GET: Index
        public ActionResult Index(string searchString)
        {

            var courses = from c in db.courses
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.courseName.Contains(searchString));
            }

            return View("RegistCourse", courses.ToList());
        }

        public JsonResult CheckEnrollment(string courseId)
        {
            bool success = false;
            string message = "";
            if (Session["StudentId"] == null)
            {
                message = "Session expired. Please log in again.";
            }
            else
            {
                var studentId = Session["StudentId"].ToString();

                // Gọi phương thức để kiểm tra xem học sinh đã ghi danh vào khóa học chưa
                bool isEnrolled = IsUserEnrolled(studentId, courseId);
                if (isEnrolled)
                {
                    message = "You have already enrolled in this course.";
                }
                else
                {
                    message = "Regist course success";
                    enrollment en = new enrollment()
                    {
                        courseId = courseId,
                        courseName = "123",
                        studentId = studentId,
                        studentName = "123",
                        semester = "2",
                        enrollmentDate = DateTime.Now
                    };
                    try
                    {
                        db.enrollments.Add(en);
                        db.SaveChanges();
                        success = true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
            }

            // Trả về kết quả dưới dạng JSON
            return Json(new { success = success, message = message });
        }

        // Phương thức để kiểm tra trạng thái ghi danh
        private bool IsUserEnrolled(string userId, string courseId)
        {
            // Logic kiểm tra ghi danh trong cơ sở dữ liệu
            using (var context = new StuMEntities())
            {
                // Kiểm tra xem có bản ghi nào trong bảng Enrollments khớp với userId và courseId hay không
                return context.enrollments.Any(e => e.studentId.Equals(userId) && e.courseId.Equals(courseId));
            }
        }
    }
}
