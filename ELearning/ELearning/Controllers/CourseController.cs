using ELearning.Models;

using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using System.Net;

namespace ELearning.Controllers
{
    public class CourseController : Controller
    {
        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
        private readonly ILogger<CourseController> _logger;

        public CourseController(ILogger<CourseController> logger)
        {
            _logger = logger;
        }

       
        public IActionResult AddCourse()
        {
            InstructorModel instructorModel = new InstructorModel();
            instructorModel.GetInstructors();

            ViewBag.SqlData = instructorModel;

            return View(new CourseModel());
        }
       
        public IActionResult ListCourse()
        {
            CourseModel sqldata = new CourseModel();
            sqldata.Getcourses();

            ViewBag.sqldata = sqldata;
            return View();
        }


        [HttpPost]
        public IActionResult AddCourse(CourseModel model)
        {
            /* Make sure that inputted value isn't null or empty */
            if (String.IsNullOrEmpty(model.CourseName) || model.InstructorId == 0)
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {

                CourseModel getFormData = new CourseModel();
                getFormData.AddCourse(model);


                ViewBag.Message = "Success: Value will be inserted into database";
                return Redirect("ListCourse");
            }
        }
       
        // POST: Course/Delete/5

        public ActionResult DeleteConfirmed(int id)
        {
            using (OracleConnection connection = new OracleConnection(conString))
            {
                connection.Open();

                string query = "DELETE FROM Course WHERE CourseId = :courseId";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("courseId", id));
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("ListCourse"); // Assuming "Index" is the action method that displays the course list
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
