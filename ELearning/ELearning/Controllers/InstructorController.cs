using ELearning.Models;

using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace ELearning.Controllers
{
    public class InstructorController : Controller
    {
        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
        private readonly ILogger<InstructorController> _logger;

        public InstructorController(ILogger<InstructorController> logger)
        {
            _logger = logger;
        }


        public IActionResult AddInstructor()
        {
            return View(new InstructorModel());
        }

        public IActionResult ListInstructor()
        {
            InstructorModel sqldata = new InstructorModel();
            sqldata.GetInstructors();

            ViewBag.sqldata = sqldata;
            return View();
        }


        [HttpPost]
        public IActionResult AddInstructor(InstructorModel model)
        {
            /* Make sure that inputted value isn't null or empty */
            if (String.IsNullOrEmpty(model.InstructorName))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {

                InstructorModel getFormData = new InstructorModel();
                getFormData.AddInstructor(model);


                ViewBag.Message = "Success: Value will be inserted into database";
                return Redirect("ListInstructor");
            }
        }
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "DELETE FROM Instructor WHERE InstructorId = :InstructorId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("InstructorId", id));
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("ListInstructor");
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
                    return RedirectToAction("ListInstructor");
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
