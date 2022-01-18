using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSINTBrowser
{
    public class DbConnect
    {
        private string connectionString;
        SqlConnection cnn;


        public void open_connection()
        {
            connectionString = @"Data Source=LAPTOP-8VDVE6H0;Initial Catalog=OSIBDatabase;Integrated Security=True";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            Console.WriteLine("Connection Open");
        }

        public void close_connection()
        {
            cnn.Close();
        }

        public void save_to_database(DateTime captureDate, string captureName, string captureLocation)
        {
            SqlCommand cmd;
            //SqlDataAdapter adapter = new SqlDataAdapter();
            string investigatorName = "Matt";
            byte[] filepath = Encoding.ASCII.GetBytes(captureLocation);

            string query = "INSERT INTO Testcase (date_time, investigator_name, screenshot_name, filepath) " +
                "VALUES (@date_time, @investigator_name, @screenshot_name, @filepath)";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@date_time", captureDate);
                cmd.Parameters.AddWithValue("@investigator_name", investigatorName);
                cmd.Parameters.AddWithValue("@screenshot_name", captureName);
                cmd.Parameters.Add("@filepath", SqlDbType.VarBinary).Value = filepath;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

        }
    }

}
