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

        //Opens connection to the database - uses machine name.
        public void open_connection()
        {
            string connDataSource = Environment.MachineName;
            connectionString = @"Data Source="+connDataSource+";Initial Catalog=OSIBDatabase;Integrated Security=True";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            Console.WriteLine("Connection Open");
        }

        public void close_connection()
        {
            cnn.Close();
        }

        public void captureToDatabase(DateTime captureDate, string desc, string source, string captureLocation, bool? check)
        {
            int caseid = Case.caseID;
            int userid = Case.userID;
            //string evidescription = "This is a test of a description for a screenshot";
            //string source = "https://www.sourcelinktest.com/profilename";
            var hashOfFile = 76543;

            SqlCommand cmd;
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //int caseid = get_caseID();
            byte[] filepath = Encoding.ASCII.GetBytes(captureLocation);

            string query = "INSERT INTO Evidence (caseID, userID, eviDesc, indecentFlag, captureDate, sourceLink, filePath, fileHash) " +
                "VALUES (@caseID, @userID, @eviDesc, @indecentFlag, @captureDate, @sourceLink, @filepath, @fileHash)";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@caseID", caseid);
                cmd.Parameters.AddWithValue("@userID", userid);
                cmd.Parameters.AddWithValue("@eviDesc", desc);
                cmd.Parameters.AddWithValue("@indecentFlag", check);
                cmd.Parameters.AddWithValue("@captureDate", captureDate);
                cmd.Parameters.AddWithValue("sourceLink", source);
                cmd.Parameters.Add("@filepath", SqlDbType.VarBinary).Value = filepath;
                cmd.Parameters.AddWithValue("@fileHash", hashOfFile);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            cnn.Close();
            

        }
        
        public void addNewCase(DateTime creationDate, string subjectName, string description)
        {
            int userid = getUserId();
            SqlCommand cmd;
            //**TEMP VARIABLES WHILE TESTING**
            string tempEncryptionKey = "1111";

            byte[] tempKey = Encoding.ASCII.GetBytes(tempEncryptionKey);

            string query = "INSERT INTO Cases (userID, caseName, caseDesc, caseStatus, dateCreated, dateAccessed, encryptionKey)" +
                "VALUES (@userID, @caseName, @caseDesc, @caseStatus, @dateCreated, @dateAccessed, @key)";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@userID", userid);
                cmd.Parameters.AddWithValue("@caseName", subjectName);
                cmd.Parameters.AddWithValue("@caseDesc", description);
                cmd.Parameters.AddWithValue("@caseStatus", 1);
                cmd.Parameters.AddWithValue("@dateCreated", creationDate);
                cmd.Parameters.AddWithValue("@dateAccessed", creationDate);
                cmd.Parameters.Add("@key", SqlDbType.VarBinary).Value = tempKey;
                //cmd.Parameters.Add("@filepath", SqlDbType.VarBinary).Value = filepath;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }

        public int getUserId()
        {
            //**TEMP HARD CODED USERNAME TO GET ID**
            string invest = "saraTest";
            int id = 0;
            open_connection();
            SqlCommand cmd;
            string query = "SELECT userID FROM Users WHERE username = @name";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@name", invest);
                //cmd.ExecuteReader();
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

            }
            return id;
        }

        //public int getCaseId()
        //{
        //    int id = 0;
        //    open_connection();
        //    string caseName = Case.caseName;
        //    SqlCommand cmd;
        //    string query = "SELECT caseID FROM Cases WHERE caseName = @name";
        //    using (cmd = new SqlCommand(query, cnn))
        //    {
        //        cmd.Parameters.AddWithValue("@name", caseName);
        //        //cmd.ExecuteReader();
        //        id = Convert.ToInt32(cmd.ExecuteScalar());
        //        cmd.Dispose();

        //    }
        //    return id;
        //}

        public void getTheCase(string caseName)
        {
            open_connection();
            SqlCommand cmd;
            string query = "SELECT caseID, userID, caseName FROM Cases WHERE caseName = @name";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@name", caseName);
                SqlDataReader reading = cmd.ExecuteReader();
                while (reading.Read())
                {
                    Case.caseID = Convert.ToInt32(reading["caseID"]);
                    Case.userID = Convert.ToInt32(reading["userID"]);
                    Case.caseName = reading["caseName"].ToString();
                }
                cmd.Dispose();

            }
        }
    }

}
