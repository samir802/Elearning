using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ELearning.Models
{
   public class InstructorModel
    {
        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }

        public string CourseId { get; set; }
        public string CourseTitle { get; set; }

        public List<InstructorModel> InstructorList = new List<InstructorModel>();

        public List<InstructorModel> SearchByInstructor = new List<InstructorModel>();

        public void AddInstructor(InstructorModel insModel)
        {
            try
            {
                using(OracleConnection con = new OracleConnection(conString))
                {
                    string query = "INSERT INTO Instructor(InstructorName) Values ('" + insModel.InstructorName + "')";
                    OracleCommand cmd  = new OracleCommand(query, con);

                    Console.WriteLine(insModel.InstructorName);
                    con.Open();
                    cmd.ExecuteReader();
                    con.Close();
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void GetInstructors()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT * FROM Instructor";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database, and select the data.
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        InstructorModel sm = new InstructorModel();
                        sm.InstructorId = reader.GetInt32(0);
                        sm.InstructorName = reader.GetString(1);

                        InstructorList.Add(sm);

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

        public InstructorModel GetInstructorDetails(int id)
        {
            InstructorModel instructor = new InstructorModel();

            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "SELECT InstructorId, InstructorName FROM Instructor WHERE InstructorId = :InstructorId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("InstructorId", id));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                instructor.InstructorId = Convert.ToInt32(reader["InstructorId"]);
                                instructor.InstructorName = Convert.ToString(reader["InstructorName"]);
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error fetching instructor details: " + ex.Message);
            }

            return instructor;
        }

        public void GetInstructorBySearchName(string instructorName)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT c.CourseId, c.CourseName,\r\n       CASE \r\n           WHEN course_count >= 2 THEN \r\n               i.InstructorId\r\n           ELSE 0\r\n       END AS InstructorId,\r\n       CASE \r\n           WHEN course_count >= 2 THEN \r\n               i.InstructorName\r\n           ELSE 'Empty'\r\n       END AS InstructorName\r\nFROM course c\r\nJOIN instructor i ON c.InstructorId = i.InstructorId\r\nLEFT JOIN (\r\n    SELECT InstructorId, COUNT(*) AS course_count\r\n    FROM course\r\n    GROUP BY InstructorId\r\n) c_count ON c.InstructorId = c_count.InstructorId\r\nWHERE i.InstructorName = :instructorName\r\nGROUP BY c.CourseId, c.CourseName, c_count.course_count, i.InstructorId, i.InstructorName";

                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.Parameters.Add(":instructorName", OracleDbType.Varchar2).Value = instructorName;

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database and select the data
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        InstructorModel instructor = new InstructorModel();


                        // Handle potential NULL values for COURSEID and COURSETITLE
                        if (!reader.IsDBNull(0))
                            instructor.CourseId = reader.GetString(0);

                        if (!reader.IsDBNull(1))
                            instructor.CourseTitle = reader.GetString(1);

                        instructor.InstructorId = reader.GetInt32(2);
                        instructor.InstructorName = reader.GetString(3);
                        SearchByInstructor.Add(instructor);
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
