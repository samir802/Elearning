using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics.Metrics;

namespace ELearning.Models
{
    public class ProgressModel
    {

        string conString = DbConnection.conString;


        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string? LessonStatus { get; set; }
        public DateTime LastAccessedDate { get; set; }

        public string? StudentName { get; set; }
        public string? LessonTitle { get; set; }
        public string? CourseName { get; set; }

        public int EnrollmentId { get; set; }

        public List<ProgressModel> ProgressList = new List<ProgressModel>();


        public void GetEnrollmentDetailsByEnrollStudentIdToAddProgress(int StudentId)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT e.EnrollmentId, s.StudentId, s.StudentName, c.CourseId, c.CourseName, l.LessonId, l.LessonTitle " +
                      "FROM enrollment e " +
                      "JOIN student s ON e.StudentId = s.StudentId " +
                      "JOIN course c ON e.CourseId = c.CourseId " +
                      "LEFT JOIN lesson l ON c.CourseId = l.CourseId " +
                      "WHERE e.StudentId = :StudentId";


                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.Parameters.Add(":StudentId", OracleDbType.Int32).Value = StudentId;

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database and select the data
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProgressModel sm = new ProgressModel();
                        sm.EnrollmentId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        sm.StudentId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        sm.StudentName = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        sm.CourseId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        sm.CourseName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        sm.LessonId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        sm.LessonTitle = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        ProgressList.Add(sm);
                        
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


        public void GetProgress()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT " +
                        "pd.StudentID,s.StudentName, pd.LessonId, l.LessonTitle, pd.LessonStatus," +
                        " pd.LastAccessedDate, pd.CourseId, c.CourseName FROM progress_details pd " +
                        "JOIN student s ON pd.StudentID = s.StudentId " +
                        "JOIN lesson l ON pd.LessonId = l.LessonId " +
                        "JOIN course c ON pd.CourseId = c.CourseId";


                    OracleCommand cmd = new OracleCommand(queryString, con);

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database and select the data
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProgressModel sm = new ProgressModel();
                        sm.StudentId = reader.GetInt32(0);
                        sm.StudentName = reader.GetString(1);
                        sm.LessonId = reader.GetInt32(2);
                        sm.LessonTitle = reader.GetString(3);
                        sm.LessonStatus = reader.GetString(4);
                        sm.LastAccessedDate = reader.GetDateTime(5);
                        sm.CourseId = reader.GetInt32(6);
                        sm.CourseName = reader.GetString(7);
                        ProgressList.Add(sm);
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
