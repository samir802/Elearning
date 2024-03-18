using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace ELearning.Controllers
{
	public class EnrollmentController : Controller
	{
		string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
		private readonly ILogger<EnrollmentController> _logger;

		public EnrollmentController(ILogger<EnrollmentController> logger)
		{
			_logger = logger;
		}

		public IActionResult AddEnrollment()
		{
			StudentModel sqldata = new StudentModel();
			sqldata.GetStudents();

			CourseModel course = new CourseModel();
			course.Getcourses();

			ViewBag.sqldata = sqldata;
			ViewBag.course = course; 


			return View(new EnrollmentModel());
		}

		public IActionResult ListEnrollment()
		{

			EnrollmentModel EnrollmentModel = new EnrollmentModel();
			EnrollmentModel.GetEnrollment();
			ViewBag.SqlData = EnrollmentModel;

			return View();
		}

        [HttpGet]
        public IActionResult ListEnrollment(string searchName)
        {
            EnrollmentModel sqldata = new EnrollmentModel();

            if (!string.IsNullOrEmpty(searchName))
            {
                // Filter the enrollment list based on the entered student name
                sqldata.GetEnrollmentsByStudentName(searchName);
            }

            else
            {
                sqldata.GetEnrollment();
            }


            ViewBag.sqldata = sqldata;
            return View();
        }

        public IActionResult PopularCourse(int month)
        {
            EnrollmentModel enrollModel = new EnrollmentModel();
            List<EnrollmentModel> topThreeCourses = enrollModel.PopularCourse(month);
            ViewBag.SelectedMonth = month;
            return View(topThreeCourses);
        }




        [HttpPost]
		public IActionResult AddEnrollment(EnrollmentModel model)
		{
			/* Make sure that inputted value isn't null or empty */
			if (model.StudentId == null || model.CourseId == null)
			{
				ViewBag.Message = "Error: Don't submit an empty value.";
				return View();
			}
			else
			{

				EnrollmentModel getFormData = new EnrollmentModel();
				getFormData.AddEnrollment(model);


				ViewBag.Message = "Success: Value will be inserted into database";
				return Redirect("ListEnrollment");
			}
		}

		public IActionResult GetEnrollmentById(int id)
		{
			EnrollmentModel enroll = new EnrollmentModel();
			enroll = enroll.GetEnrollmentDetails(id);

			StudentModel student = new StudentModel();
			student.GetStudents();
			ViewBag.Students = student;

			CourseModel course = new CourseModel();
			course.Getcourses();
			ViewBag.Course = course;

			return View("SearchDetails", enroll);
		}


		public IActionResult GetSpecificEnrollmentDetails(int id)
		{
            EnrollmentModel enroll = new EnrollmentModel();
            enroll = enroll.GetEnrollmentDetails(id);
            return View("SpecificEnrollmentDetails", enroll);

        }

        [HttpPost]
        public ActionResult Update(EnrollmentModel updatedEnrollment)
        {
            using (OracleConnection conn = new OracleConnection(conString))
            {
                conn.Open();
                string query = "UPDATE Enrollment SET StudentId=:StudentId, CourseId=:CourseId,EnrollmentDate=:EnrollmentDate WHERE EnrollmentId = :EnrollmentId";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":StudentId", OracleDbType.Int32).Value = updatedEnrollment.StudentId;
                cmd.Parameters.Add(":CourseIdId", OracleDbType.Int32).Value = updatedEnrollment.CourseId;
                cmd.Parameters.Add(":CourseIdId", OracleDbType.Date).Value = updatedEnrollment.EnrollmentDate;               
                cmd.Parameters.Add(":EnrollmentId", OracleDbType.Int32).Value = updatedEnrollment.EnrollmentId;

                cmd.ExecuteNonQuery();
            }
            // Redirect back to the page or any other appropriate action
            return RedirectToAction("ListEnrollment", "Enrollment"); 
        }

        public ActionResult DeleteConfirmed(int id)
		{
			using (OracleConnection connection = new OracleConnection(conString))
			{
				connection.Open();

				string query = "DELETE FROM Enrollment WHERE EnrollmentId = :EnrollmentId";
				using (OracleCommand command = new OracleCommand(query, connection))
				{
					command.Parameters.Add(new OracleParameter("EnrollmentId", id));
					command.ExecuteNonQuery();
				}
			}

			return RedirectToAction("ListEnrollment"); 
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
