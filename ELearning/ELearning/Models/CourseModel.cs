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



    }
}
