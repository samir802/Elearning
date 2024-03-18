using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ELearning.Models
{
    public class LessonModel
    {
        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";


        public int LessonId { get; set; }
        public string? LessonTitle { get; set; }
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        public List<LessonModel> lessons = new List<LessonModel>();
         
        public void Getlessons()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT l.LessonId,l.LessonTitle,c.Courseid,c.CourseName FROM Lesson l INNER JOIN Course c ON l.CourseId = c.CourseId";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database, and select the data.
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LessonModel sm = new LessonModel();
                        sm.LessonId = reader.GetInt32(0);
                        sm.LessonTitle = reader.GetString(1);
                        sm.CourseId = reader.GetInt32(2);
                        sm.CourseName = reader.GetString(3);
                        lessons.Add(sm);

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
        public void AddLesson(LessonModel lesson)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "INSERT INTO Lesson (LessonTitle, CourseId) " +
                                         "VALUES (:LessonTitle, :CourseId)";
                    OracleCommand cmd = new OracleCommand(queryString, con);

                    // Using parameters to avoid SQL injection
                    cmd.Parameters.Add(":LessonName", OracleDbType.Varchar2).Value = lesson.LessonTitle;                  
                    cmd.Parameters.Add(":CourseId", OracleDbType.Int32).Value = lesson.CourseId;

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


        public LessonModel GetLessonDetails(int id)
        {
            LessonModel lesson = new LessonModel();

            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "SELECT l.LessonId, l.LessonTitle, c.CourseId,c.CourseName FROM Lesson l JOIN Course c ON l.CourseId = c.CourseId WHERE l.LessonId = :LessonId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("LessonId", id));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lesson.LessonId = Convert.ToInt32(reader["LessonId"]);
                                lesson.LessonTitle = Convert.ToString(reader["LessonTitle"]);
                                lesson.CourseId = Convert.ToInt32(reader["CourseId"]);
                                lesson.CourseName = Convert.ToString(reader["LessonTitle"]);
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

            return lesson;
        }
    }
}
