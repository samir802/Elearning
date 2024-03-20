
using ELearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace ELearning.Controllers
{
    public class ProgressController : Controller
    {
        string conString = DbConnection.conString;

        
        private readonly ILogger<ProgressController> _logger;

        public ProgressController(ILogger<ProgressController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddProgress()
        {
            EnrollmentModel student = new EnrollmentModel();
            student.GetEnrollment();
            ViewBag.Student = student;
            CourseModel course = new CourseModel();
            course.Getcourses();
            ViewBag.Course = course;
            LessonModel lesson = new LessonModel();
            lesson.Getlessons();
            ViewBag.Lesson = lesson;


            return View(new ProgressModel());
        }

        public IActionResult ListProgress()
        {
            ProgressModel sqldata = new ProgressModel();
            sqldata.GetProgress();
            ViewBag.sqldata = sqldata;


            return View();
        }

        [HttpGet]
        public IActionResult CompleteAddProgress(int StudentId)
        {
            ProgressModel sqldata = new ProgressModel();

            if (StudentId != 0)
            {
                // Filter the enrollment list based on the entered student name
                sqldata.GetEnrollmentDetailsByEnrollStudentIdToAddProgress(StudentId);
            }

            ViewBag.sqldata = sqldata;
            return View();
        }


        [HttpPost]
        public IActionResult AddProgress(ProgressModel model)
        {
            /* Make sure that inputted value isn't null or empty */
            if (model.StudentId == 0|| model.LessonId == 0|| String.IsNullOrEmpty(model.LessonStatus) || model.CourseId == 0 )
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {

                ProgressModel getFormData = new ProgressModel();
                getFormData.AddProgress(model);

                
                ViewBag.Message = "Success: Value will be inserted into database";
                return Redirect("ListProgress");
            }
        }


        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
