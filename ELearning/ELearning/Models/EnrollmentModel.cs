
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;

namespace ELearning.Models
{
	public class EnrollmentModel
	{
		string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
		public int EnrollmentId { get; set; }
		public int StudentId { get; set; }
		public string StudentName { get; set; }
		public int CourseId { get; set; }
		public string CourseName { get; set; }
		public String EnrollmentDate { get; set; }

		public List<EnrollmentModel> listEnrollment =	new List<EnrollmentModel>();

		public void AddEnrollment(EnrollmentModel model)
		{

			try
			{
				using(OracleConnection con = new OracleConnection(conString))
				{
					string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
					string query = "INSERT INTO Enrollment(StudentId,CourseId,EnrollmentDate)"+ "VALUES (:StudentId,:CourseId,:EnrollmentId)";
					OracleCommand cmd = new OracleCommand(query, con);

					// Using parameters to avoid SQL injection
					cmd.Parameters.Add(":StudentId", OracleDbType.Int32).Value = model.StudentId;
					cmd.Parameters.Add(":CourseId", OracleDbType.Int32).Value = model.CourseId;
					cmd.Parameters.Add(":EnrollmentDate", OracleDbType.Varchar2).Value = currentDate;

					Console.WriteLine(model.StudentId);

					cmd.CommandType = CommandType.Text;

					con.Open();
					cmd.ExecuteNonQuery();

				}

			}catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

		}
		public void GetEnrollment()
		{
			try
			{
				using (OracleConnection con = new OracleConnection(conString))
				{
					string queryString = "SELECT e.EnrollmentId, s.StudentId, c.CourseId,e.EnrollmentDate, s.StudentName, c.CourseName " +
						"FROM enrollment e JOIN student s ON e.StudentId = s.StudentId JOIN course c ON e.CourseId = c.CourseId";
					OracleCommand cmd = new OracleCommand(queryString, con);
					cmd.BindByName = true;
					cmd.CommandType = CommandType.Text;

					// Connect with the database, and select the data.
					con.Open();
					OracleDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						EnrollmentModel sm = new EnrollmentModel();
						sm.EnrollmentId = reader.GetInt32(0);
						sm.StudentId = reader.GetInt32(1);
						sm.CourseId = reader.GetInt32(2);
						sm.EnrollmentDate = reader.GetString(3);
						sm.StudentName = reader.GetString(4);
						sm.CourseName = reader.GetString(5);
						listEnrollment.Add(sm);

					}

					reader.Dispose();
					con.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
		}
	}
}
