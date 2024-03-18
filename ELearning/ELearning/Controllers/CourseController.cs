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


        public IActionResult GetCourseById(int id)
        {
            CourseModel course = new CourseModel();
            course = course.GetCourseDetails(id);

            InstructorModel instructorModel = new InstructorModel();
            instructorModel.GetInstructors();
            ViewBag.SqlData = instructorModel;

            return View("CourseDetails", course);
        }

        [HttpPost]
        public ActionResult Update(CourseModel updatedCourse)
        {
            using (OracleConnection conn = new OracleConnection(conString))
            {
                conn.Open();
                string query = "UPDATE Course SET CourseName=:CourseName, InstructorId=:InstructorId WHERE CourseId = :CourseId";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":CourseName", OracleDbType.Varchar2).Value = updatedCourse.CourseName;
                cmd.Parameters.Add(":InstructorId", OracleDbType.Int32).Value = updatedCourse.InstructorId;
                cmd.Parameters.Add(":CourseId", OracleDbType.Int32).Value = updatedCourse.CourseId;

                Console.WriteLine(updatedCourse.CourseId);
                Console.WriteLine(updatedCourse.InstructorId);

                cmd.ExecuteNonQuery();
            }
            // Redirect back to the page or any other appropriate action
            return RedirectToAction("ListCourse", "Course"); // Redirect to Home/Index
        }
        public ActionResult DeleteConfirmed(int id)
        {
            try
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

                return RedirectToAction("ListCourse");
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-02292")) // Check if the error message contains ORA-02292
                {
                    // Handle the constraint violation by returning a view with JavaScript for pop-up dialog
                    string errorMessage = "This item cannot be deleted because it has associated child records.";
                    ViewBag.ErrorMessage = errorMessage;
                    return View("ErrorPopup");
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    return RedirectToAction("ListCourse");
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
