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
