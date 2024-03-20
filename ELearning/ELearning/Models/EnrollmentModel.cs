
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;

namespace ELearning.Models
{
	public class EnrollmentModel
	{
        string conString = DbConnection.conString;

        public int EnrollmentId { get; set; }
		public DateTime EnrollmentDate { get; set; }

		public int StudentId { get; set; }
		public string? StudentName { get; set; }
        public string? Contact { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }

        public string? Country { get; set; }
        public int Zip {  get; set; }


        public int CourseId { get; set; }
		public string? CourseName { get; set; }

        public string? LessonTitle { get; set; }




        public string? InstructorName { get; set; }
        public int EnrollmentCount { get; set; }

        public List<EnrollmentModel> listEnrollment =	new List<EnrollmentModel>();

		public void AddEnrollment(EnrollmentModel model)
		{

			try
			{
				using(OracleConnection con = new OracleConnection(conString))
				{
					string query = "INSERT INTO Enrollment(StudentId,CourseId,EnrollmentDate)"+ "VALUES (:StudentId,:CourseId,:EnrollmentDate)";
					OracleCommand cmd = new OracleCommand(query, con);

					// Using parameters to avoid SQL injection
					cmd.Parameters.Add(":StudentId", OracleDbType.Int32).Value = model.StudentId;
					cmd.Parameters.Add(":CourseId", OracleDbType.Int32).Value = model.CourseId;
					cmd.Parameters.Add(":EnrollmentDate", OracleDbType.Date).Value = DateTime.Now.Date;

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
					string queryString = "SELECT e.EnrollmentId,e.StudentId,s.StudentName,s.contact,s.DOB,s.email,co.country_name,c.CourseId,c.CourseName,i.InstructorName,e.EnrollmentDate FROM enrollment e " +
						"JOIN student s ON e.StudentId = s.StudentId " +
						"JOIN course c ON e.CourseId = c.CourseId LEFT " +
						"JOIN instructor i ON c.InstructorId = i.InstructorId " +
						"JOIN country co ON s.zip = co.zip";

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
						sm.StudentName = reader.GetString(2);
						sm.Contact = reader.GetString(3);
						sm.DOB = reader.GetDateTime(4);
						sm.Email = reader.GetString(5);
						sm.Country = reader.GetString(6);
                        sm.CourseId = reader.GetInt32(7);
                        sm.CourseName = reader.GetString(8);
                        sm.InstructorName = reader.GetString(9);
                        sm.EnrollmentDate = reader.GetDateTime(10);
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
        public EnrollmentModel GetEnrollmentDetails(int id)
        {
            EnrollmentModel enroll = new EnrollmentModel();

            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "SELECT e.EnrollmentId,e.StudentId,s.StudentName,s.contact,s.DOB,s.email,co.Zip,co.Country_Name,c.CourseId,c.CourseName,i.InstructorName,e.EnrollmentDate FROM enrollment e " +
                        "JOIN student s ON e.StudentId = s.StudentId " +
                        "JOIN course c ON e.CourseId = c.CourseId LEFT " +
                        "JOIN instructor i ON c.InstructorId = i.InstructorId " +
                        "JOIN country co ON s.zip = co.zip"+
                        " WHERE e.EnrollmentId = :EnrollmentId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("EnrollmentId", id));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                enroll.EnrollmentId = Convert.ToInt32(reader["EnrollmentId"]);
                                enroll.StudentId = Convert.ToInt32(reader["StudentId"]);
                                enroll.StudentName = Convert.ToString(reader["StudentName"]);
                                enroll.Contact = Convert.ToString(reader["Contact"]);
                                enroll.DOB = reader.GetDateTime(reader.GetOrdinal("DOB"));
                                enroll.Email = Convert.ToString(reader["Email"]);
                                enroll.Zip = Convert.ToInt32(reader["Zip"]);
                                enroll.Country = Convert.ToString(reader["Country_Name"]);
                                enroll.CourseId = Convert.ToInt32(reader["CourseId"]);
                                enroll.CourseName = Convert.ToString(reader["CourseName"]);
                                enroll.EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate"));



                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error fetching student details: " + ex.Message);
            }

            return enroll;
        }

        public void GetEnrollmentsByStudentName(string studentName)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT e.EnrollmentId,e.StudentId,s.StudentName,s.contact,s.DOB,s.email,co.country_name,c.CourseId,c.CourseName,i.InstructorName,e.EnrollmentDate" +
                    " FROM enrollment e " +
                    "JOIN student s ON e.StudentId = s.StudentId " +
                    "JOIN course c ON e.CourseId = c.CourseId " +
                    "LEFT JOIN instructor i ON c.InstructorId = i.InstructorId " +
                    "JOIN country co ON s.zip = co.zip " +
                    "WHERE UPPER(s.StudentName) LIKE '%' || UPPER(:studentName) || '%'";


                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.Parameters.Add(":studentName", OracleDbType.Varchar2).Value = studentName;

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database and select the data
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EnrollmentModel sm = new EnrollmentModel();
                        sm.EnrollmentId = reader.GetInt32(0);
                        sm.StudentId = reader.GetInt32(1);
                        sm.StudentName = reader.GetString(2);
                        sm.Contact = reader.GetString(3);
                        sm.DOB = reader.GetDateTime(4);
                        sm.Email = reader.GetString(5);
                        sm.Country = reader.GetString(6);
                        sm.CourseId = reader.GetInt32(7);
                        sm.CourseName = reader.GetString(8);
                        sm.InstructorName = reader.GetString(9);
                        sm.EnrollmentDate = reader.GetDateTime(10);
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

        public List<EnrollmentModel> PopularCourse(int month)
        {
            List<EnrollmentModel> topThreeCourses = new List<EnrollmentModel>();

            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT se.COURSEID, c.COURSENAME, COUNT(*) AS EnrollmentCount FROM ENROLLMENT se " +
                        "JOIN COURSE c ON se.COURSEID = c.COURSEID " +
                        "WHERE EXTRACT(MONTH FROM se.EnrollmentDate) = :month GROUP BY se.COURSEID, c.COURSENAME " +
                        "ORDER BY EnrollmentCount DESC " +
                        "FETCH FIRST 3 ROWS ONLY";


                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        cmd.Parameters.Add(":month", OracleDbType.Int32).Value = month;
                        con.Open();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read() && count < 3)
                        {
                            EnrollmentModel courseEnrollment = new EnrollmentModel();
                            courseEnrollment.CourseId = reader.GetInt32(0);
                            courseEnrollment.CourseName = reader.GetString(1);
                            courseEnrollment.EnrollmentCount = reader.GetInt32(2);
                            topThreeCourses.Add(courseEnrollment);
                            count++;
                        }
                        reader.Dispose();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed (logging, rethrow, etc.)
            }

            return topThreeCourses;
        }
    }
}
