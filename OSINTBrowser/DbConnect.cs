using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


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

        //public void close_connection()
        //{
        //    cnn.Close();
        //}

        //Evidence capture - added to database.
        public void captureToDatabase(DateTime captureDate, string desc, string source, string captureLocation, bool? check, byte[] hash)
        {
            int caseid = Case.caseID;
            int userid = Case.userID;

            SqlCommand cmd;
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
                cmd.Parameters.Add("@fileHash", SqlDbType.VarBinary).Value = hash;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            cnn.Close();          
        }
        
        //Adding a new case to the database.
        public void addNewCase(DateTime creationDate, string subjectName, string description)
        {
            int userid = getUserId();
            SqlCommand cmd;
            //**TEMP VARIABLES**
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
            cnn.Close();
        }

        //Getting the user ID
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
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
            }
            return id;
            cnn.Close();
        }

        //Updating the case when it has last been accessed.
        public void updateDateAccessed(int caseID)
        {
            DateTime now = DateTime.Now;
            open_connection();
            SqlCommand cmd;
            string query = "UPDATE dbo.Cases SET dateAccessed = @now WHERE caseID = @caseID";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@now", now);
                cmd.Parameters.AddWithValue("@caseID", caseID);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            cnn.Close();        
        }

        //Updates the Case class with caseId, userId and caseName.
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
            updateDateAccessed(Case.caseID);
            cnn.Close();
        }

        //Used for the end report
        public List<String> getReport()
        {
            List<String> data = new List<string>()
                ;
            int thisCase = Case.caseID;
            open_connection();
            SqlCommand cmd;
            string query = "SELECT captureDate, sourceLink, eviDesc, filePath, indecentFlag FROM Evidence WHERE caseID = @caseID";
            using (cmd = new SqlCommand(query, cnn))
            {
                cmd.Parameters.AddWithValue("@caseID", thisCase);
                SqlDataReader reading = cmd.ExecuteReader();
                while (reading.Read())
                {
                    data.Add(reading["captureDate"].ToString());
                    data.Add(reading["sourceLink"].ToString());
                    data.Add(reading["eviDesc"].ToString());
                    data.Add(reading["filePath"].ToString());
                    data.Add(reading["indecentFlag"].ToString());
                }
            }
            cmd.Dispose();
            return data;
        }
        

    }

}
