using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace ELearning.Controllers
{
    public class LessonController : Controller
    {
        string conString = DbConnection.conString;

        
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

        public IActionResult GetLessonById(int id)
        {
            LessonModel lesson = new LessonModel();
            lesson = lesson.GetLessonDetails(id);

            CourseModel courseModel = new CourseModel();
            courseModel.Getcourses();
            ViewBag.SqlData = courseModel;

            return View("LessonDetails", lesson);
        }

        [HttpPost]
        public ActionResult Update(LessonModel updatedLesson)
        {
            using (OracleConnection conn = new OracleConnection(conString))
            {
                conn.Open();
                string query = "UPDATE Lesson SET LessonTitle=:LessonTitle, CourseId =:CourseId WHERE LessonId = :LessonId";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":LessonTitle", OracleDbType.Varchar2).Value = updatedLesson.LessonTitle;

                cmd.Parameters.Add(":CourseId", OracleDbType.Int32).Value = updatedLesson.CourseId;
                cmd.Parameters.Add(":LessonId", OracleDbType.Int32).Value = updatedLesson.LessonId;

                cmd.ExecuteNonQuery();
            }
            // Redirect back to the page or any other appropriate action
            return RedirectToAction("ListLesson", "Lesson"); // Redirect to Home/Index
        }

        public ActionResult DeleteConfirmed(int id)
        {
            try
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

                return RedirectToAction("ListLesson");
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
                    return RedirectToAction("ListLesson");
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
