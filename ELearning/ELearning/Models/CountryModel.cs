using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics.Metrics;

namespace ELearning.Models
{
    public class CountryModel
    {
        string conString = DbConnection.conString;

        public int Zip { get; set; }
        public string? Country { get; set; }

        public List<CountryModel> country = new List<CountryModel>();

        public void GetCountry()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT * FROM Country";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database, and select the data.
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CountryModel sm = new CountryModel();
                        sm.Zip = reader.GetInt32(0);
                        sm.Country = reader.GetString(1);
                        country.Add(sm);

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
