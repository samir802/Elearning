using Oracle.ManagedDataAccess.Client;
using System;

namespace ELearning.Models
{
    public class DashboardModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCompletedLessons { get; set; }

        // Additional properties as needed

        public DashboardModel()
        {
            // Default constructor
        }

        public void PopulateDashboardData(string connectionString)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();

                    // Retrieve total students count
                    string studentCountQuery = "SELECT COUNT(*) FROM STUDENT";
                    using (OracleCommand studentCountCmd = new OracleCommand(studentCountQuery, con))
                    {
                        TotalStudents = Convert.ToInt32(studentCountCmd.ExecuteScalar());
                    }

                    // Retrieve total courses count
                    string courseCountQuery = "SELECT COUNT(*) FROM Course";
                    using (OracleCommand courseCountCmd = new OracleCommand(courseCountQuery, con))
                    {
                        TotalCourses = Convert.ToInt32(courseCountCmd.ExecuteScalar());
                    }

                    // Retrieve total instructors count
                    string instructorCountQuery = "SELECT COUNT(*) FROM Instructor";
                    using (OracleCommand instructorCountCmd = new OracleCommand(instructorCountQuery, con))
                    {
                        TotalInstructors = Convert.ToInt32(instructorCountCmd.ExecuteScalar());
                    }

                    // Retrieve total completed lessons count
                    string completedLessonsCountQuery = "SELECT COUNT(*) FROM Progress_Details WHERE LessonStatus = 'Completed'";
                    using (OracleCommand completedLessonsCountCmd = new OracleCommand(completedLessonsCountQuery, con))
                    {
                        TotalCompletedLessons = Convert.ToInt32(completedLessonsCountCmd.ExecuteScalar());
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
