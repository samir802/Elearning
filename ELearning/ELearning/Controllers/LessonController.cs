using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace ELearning.Controllers
{
    public class LessonController : Controller
    {
        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
        private readonly ILogger<LessonController> _logger;

        public LessonController(ILogger<LessonController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddLesson()
        {
            CourseModel courseModel = new CourseModel();
            courseModel.Getcourses();
            ViewBag.SqlData =courseModel;

            return View(new StudentModel());
        }

        public IActionResult ListLesson()
        {
            LessonModel sqldata = new LessonModel();
            sqldata.Getlessons();

            ViewBag.sqldata = sqldata;
            return View();
        }




        [HttpPost]
        public IActionResult AddLesson(LessonModel model)
        {
            /* Make sure that inputted value isn't null or empty */
            if (String.IsNullOrEmpty(model.LessonTitle) || model.CourseId == null)
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {

                LessonModel getFormData = new LessonModel();
                getFormData.AddLesson(model);


                ViewBag.Message = "Success: Value will be inserted into database";
                return Redirect("ListLesson");
            }
        }

        public ActionResult DeleteConfirmed(int id)
        {
            using (OracleConnection connection = new OracleConnection(conString))
            {
                connection.Open();

                string query = "DELETE FROM Lesson WHERE Lessonid = :LessonId";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("LessonId", id));
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("ListLesson"); // Assuming "Index" is the action method that displays the course list
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
