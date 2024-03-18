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

        public IActionResult GetInstructorById(int id)
        {
            InstructorModel instructor = new InstructorModel();
            instructor = instructor.GetInstructorDetails(id);

            //CountryModel countryModel = new CountryModel();
            //countryModel.GetCountry();
            //ViewBag.SqlData = countryModel;

            return View("InstructorDetails", instructor);
        }

        [HttpPost]
        public ActionResult Update(InstructorModel updatedInstructor)
        {
            using (OracleConnection conn = new OracleConnection(conString))
            {
                conn.Open();
                string query = "UPDATE Instructor SET InstructorName=:InstructorName WHERE InstructorId = :InstructorId";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":InstructorName", OracleDbType.Varchar2).Value = updatedInstructor.InstructorName;
                cmd.Parameters.Add(":InstructorId", OracleDbType.Int32).Value = updatedInstructor.InstructorId;

                cmd.ExecuteNonQuery();
            }
            // Redirect back to the page or any other appropriate action
            return RedirectToAction("ListInstructor", "Instructor");
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

        public IActionResult SearchByInstructor(string searchName)
        {
            InstructorModel sqldata = new InstructorModel();

            if (!string.IsNullOrEmpty(searchName))
            {
                // Filter the enrollment list based on the entered student name
                sqldata.GetInstructorBySearchName(searchName);
            }
            else
            {
                sqldata.GetInstructors();
            }

            ViewBag.sqldata = sqldata;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
