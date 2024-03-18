using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace ELearning.Models
{

    public class StudentModel
    {


        string conString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";
        public int StudentId {  get; set; }


        public string StudentName { get; set; }

        public string Contact {  get; set; }
        public DateTime DOB { get; set; }

        public string Email { get; set; }

        public int Zip { get; set; }
        public string Country { get; set; } 

        public List<StudentModel> StudentList = new List<StudentModel>();

        public void GetStudents()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT s.StudentId, s.StudentName, s.Contact, s.DOB, s.Email, c.Zip, c.Country_Name FROM student s INNER JOIN country c ON s.Zip = c.Zip";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database, and select the data.
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StudentModel sm = new StudentModel();
                        sm.StudentId = reader.GetInt32(0);
                        sm.StudentName = reader.GetString(1);
                        sm.Contact = reader.GetString(2);
                        sm.DOB = reader.GetDateTime(3);
                        sm.Email = reader.GetString(4);
                        sm.Zip = reader.GetInt32(5);
                        sm.Country = reader.GetString(6);
                        StudentList.Add(sm);
                        
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
        public void AddStudent(StudentModel student)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "INSERT INTO STUDENT (StudentName, Contact, DOB, Email, Zip) " +
                                         "VALUES (:StudentName, :Contact, :DOB, :Email, :Zip)";
                    OracleCommand cmd = new OracleCommand(queryString, con);

                    // Using parameters to avoid SQL injection
                    cmd.Parameters.Add(":StudentName", OracleDbType.Varchar2).Value = student.StudentName;
                    cmd.Parameters.Add(":Contact", OracleDbType.Varchar2).Value = student.Contact;
                    cmd.Parameters.Add(":DOB", OracleDbType.Date).Value = student.DOB;
                    cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = student.Email;
                    cmd.Parameters.Add(":Zip", OracleDbType.Int32).Value = student.Zip;

                    cmd.CommandType = CommandType.Text;
                 
                    con.Open();
                    cmd.ExecuteNonQuery(); // Execute the command
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding student: " + ex.Message);
                // Log the exception or handle it appropriately
            }
        }

        public StudentModel GetStudentDetails(int id)
        {
            StudentModel student = new StudentModel();

            try
            {
                using (OracleConnection connection = new OracleConnection(conString))
                {
                    connection.Open();

                    string query = "SELECT s.StudentId, s.StudentName, s.Contact, s.DOB, s.Email, s.Zip,c.CountryName FROM Student s JOIN country c ON s.zip = c.zip WHERE s.StudentId = :StudentId";
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("StudentId", id));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student.StudentId = Convert.ToInt32(reader["StudentId"]);
                                student.StudentName = Convert.ToString(reader["StudentName"]);
                                student.Contact = Convert.ToString(reader["Contact"]);
                                student.DOB = reader.GetDateTime(reader.GetOrdinal("DOB"));
                                student.Email = Convert.ToString(reader["Email"]);
                                student.Zip = Convert.ToInt32(reader["Zip"]);
                                student.Country = Convert.ToString(reader["CountryName"]);
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

            return student;
        }



    }
}
