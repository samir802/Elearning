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
        public List<InstructorModel> InstructorList = new List<InstructorModel>();

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
    }
}
