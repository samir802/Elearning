
using ELearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace ELearning.Controllers
{
    public class StudentController : Controller
    {
        string conString = DbConnection.conString;

        
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddStudent()
        {
            CountryModel countryModel = new CountryModel();
            countryModel.GetCountry();
            ViewBag.SqlData = countryModel;

            return View(new StudentModel());
        }

        public IActionResult ListStudent()
        {
            StudentModel sqldata = new StudentModel();
            sqldata.GetStudents();

            ViewBag.sqldata = sqldata;
            return View();
        }




        [HttpPost]
        public IActionResult AddStudent(StudentModel model)
        {
            /* Make sure that inputted value isn't null or empty */
            if (String.IsNullOrEmpty(model.StudentName) || String.IsNullOrEmpty(model.Contact) || String.IsNullOrEmpty(model.Email) || model.Zip == null)
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {

                StudentModel getFormData = new StudentModel();
                getFormData.AddStudent(model);


                ViewBag.Message = "Success: Value will be inserted into database";
                return Redirect("ListStudent");
            }
        }


        public IActionResult GetStudentById(int id)
        {
            StudentModel student = new StudentModel();
            student = student.GetStudentDetails(id);

            CountryModel countryModel = new CountryModel();
            countryModel.GetCountry();
            ViewBag.SqlData = countryModel;

            return View("StudentDetails", student);
        }

        [HttpPost]
        public ActionResult Update(StudentModel updatedStudent)
        {
            using (OracleConnection conn = new OracleConnection(conString))
            {
                conn.Open();
                string query = "UPDATE Student SET StudentName=:StudentName, Contact=:Contact,DOB=:DOB,Email=:Email,Zip=:Zip WHERE StudentId = :StudentId";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":StudentName", OracleDbType.Varchar2).Value = updatedStudent.StudentName;
                cmd.Parameters.Add(":Contact", OracleDbType.Varchar2).Value = updatedStudent.Contact;
                cmd.Parameters.Add(":DOB", OracleDbType.Date).Value = updatedStudent.DOB;
                cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = updatedStudent.Email;
                cmd.Parameters.Add(":Zip", OracleDbType.Int32).Value = updatedStudent.Zip;
                cmd.Parameters.Add(":StudentId", OracleDbType.Int32).Value = updatedStudent.StudentId;

                cmd.ExecuteNonQuery();
            }
            // Redirect back to the page or any other appropriate action
            return RedirectToAction("ListStudent", "Student"); // Redirect to Home/Index
        }


        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "DELETE FROM Student WHERE StudentId = :StudentId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("StudentId", id));
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("ListStudent");
            }
            catch (Exception ex)
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
                    return RedirectToAction("ListStudent");
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
