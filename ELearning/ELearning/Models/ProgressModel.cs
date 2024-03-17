using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics.Metrics;

namespace ELearning.Models
{
    public class ProgressModel
    {

        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
  

        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string LessonStatus { get; set; }
        public DateTime LastAccessedDate { get; set; }

        public string StudentName { get; set; }
        public string LessonTitle { get; set; }
        public string CourseName { get; set; }

        public void AddProgress(ProgressModel model)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "INSERT INTO Progress_Details (StudentId, LessonId,LessonStatus,LastAccessedDate,CourseId) " +
                                         "VALUES (:StudentId,:LessonId,:LessonStatus,:LastAccessedDate, :CourseId)";
                    OracleCommand cmd = new OracleCommand(queryString, con);

                    // Using parameters to avoid SQL injection
                    cmd.Parameters.Add(":StudentId", OracleDbType.Int32).Value = model.StudentId;
                    cmd.Parameters.Add(":LessonId", OracleDbType.Int32).Value = model.LessonId;
                    cmd.Parameters.Add(":LessonStatus", OracleDbType.Varchar2).Value = model.LessonStatus;
                    cmd.Parameters.Add(":LastAccessedDate", OracleDbType.Date).Value = model.LastAccessedDate;
                    cmd.Parameters.Add(":CourseId", OracleDbType.Int32).Value = model.CourseId;

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.ExecuteNonQuery(); // Execute the command
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding Lesson: " + ex.Message);
                // Log the exception or handle it appropriately
            }
        }
    }
}
