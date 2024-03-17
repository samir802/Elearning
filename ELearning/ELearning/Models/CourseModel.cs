using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;
using System.Windows.Input;

namespace ELearning.Models
{
   public class CourseModel
    {
        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
        public int CourseId {  get; set; }
        public string CourseName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
		public List<CourseModel> courseList = new List<CourseModel>();

		public void AddCourse(CourseModel model)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "INSERT INTO Course (CourseName, InstructorId) " +
                                         "VALUES (:CourseName, :InstructorId)";
                    OracleCommand cmd = new OracleCommand(queryString, con);

                    // Using parameters to avoid SQL injection
                    cmd.Parameters.Add(":CourseName", OracleDbType.Varchar2).Value = model.CourseName;
                    cmd.Parameters.Add(":InstructorId", OracleDbType.Varchar2).Value = model.InstructorId;

                    cmd.CommandType = CommandType.Text;

                   
                    con.Open();
                    cmd.ExecuteNonQuery(); // Execute the command
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }

        

        public void Getcourses()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT c.CourseId,c.CourseName,i.InstructorId,i.InstructorName FROM course c INNER JOIN Instructor i on c.InstructorId = i.InstructorId";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database, and select the data.
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CourseModel sm = new CourseModel();
                        sm.CourseId = reader.GetInt32(0);
                        sm.CourseName = reader.GetString(1);
                        sm.InstructorId = reader.GetInt32(2);
                        sm.InstructorName = reader.GetString(3);
                       
                        courseList.Add(sm);

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

        public CourseModel GetCourseDetails(int id)
        {
            CourseModel course = new CourseModel();

            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "SELECT c.CourseId, c.CourseName, i.InstructorId, i.InstructorName FROM course c LEFT JOIN instructor i ON c.InstructorId = i.InstructorId WHERE c.CourseId = :CourseId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("CourseId", id));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                course.CourseId = Convert.ToInt32(reader["CourseId"]);
                                course.CourseName = Convert.ToString(reader["CourseName"]);
                                course.InstructorId = Convert.ToInt32(reader["InstructorId"]);
                                course.InstructorName = Convert.ToString(reader["InstructorName"]);
                               
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

            return course;
        }



    }
}
