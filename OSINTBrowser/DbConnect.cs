using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OSINTBrowser
{
    //Database Class.
    public class DbConnect
    {
        private string _connectionString;
        SqlConnection _cnn;

        //Opens connection to the database - uses machine name.
        public void Open_connection()
        {
            try
            {
                string connDataSource = Environment.MachineName;
                _connectionString = @"Data Source=" + connDataSource + ";Initial Catalog=OSIBDatabase;Integrated Security=True";
                _cnn = new SqlConnection(_connectionString);
                _cnn.Open();
                Console.WriteLine("Connection Open");
            }catch (Exception ex)
            {
                Console.WriteLine("Failed to open connection", ex);
            }

        }

        //Evidence capture - added to database.
        public void CaptureToDatabase(DateTime captureDate, string desc, string source, string captureLocation, bool? check, byte[] hash)
        {
            int caseid = Case.caseID;
            int userid = Case.userID;

            SqlCommand cmd;
            byte[] filepath = Encoding.ASCII.GetBytes(captureLocation);

            string query = "INSERT INTO Evidence (caseID, userID, eviDesc, indecentFlag, captureDate, sourceLink, filePath, fileHash) " +
                "VALUES (@caseID, @userID, @eviDesc, @indecentFlag, @captureDate, @sourceLink, @filepath, @fileHash)";
            using (cmd = new SqlCommand(query, _cnn))
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
            _cnn.Close();          
        }
        
        //Adding a new case to the database.
        public void AddNewCase(DateTime creationDate, string subjectName, string description)
        {
            int userid = GetUserId();
            SqlCommand cmd;
            //**TEMP VARIABLES**
            string tempEncryptionKey = "1111";

            byte[] tempKey = Encoding.ASCII.GetBytes(tempEncryptionKey);

            string query = "INSERT INTO Cases (userID, caseName, caseDesc, caseStatus, dateCreated, dateAccessed, encryptionKey)" +
                "VALUES (@userID, @caseName, @caseDesc, @caseStatus, @dateCreated, @dateAccessed, @key)";
            using (cmd = new SqlCommand(query, _cnn))
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
            _cnn.Close();
        }

        //Getting the user ID.
        public int GetUserId()
        {
            //**TEMP HARD CODED USERNAME TO GET ID**
            string invest = "saraTest";
            int id = 0;
            Open_connection();
            SqlCommand cmd;
            string query = "SELECT userID FROM Users WHERE username = @name";
            using (cmd = new SqlCommand(query, _cnn))
            {
                cmd.Parameters.AddWithValue("@name", invest);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
            }
            return id;
        }

        //Updating the case when it has last been accessed.
        public void UpdateDateAccessed(int caseID)
        {
            DateTime now = DateTime.Now;
            Open_connection();
            SqlCommand cmd;
            string query = "UPDATE dbo.Cases SET dateAccessed = @now WHERE caseID = @caseID";
            using (cmd = new SqlCommand(query, _cnn))
            {
                cmd.Parameters.AddWithValue("@now", now);
                cmd.Parameters.AddWithValue("@caseID", caseID);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            _cnn.Close();        
        }

        //Updates the Case class with caseId, userId and caseName.
        public void GetTheCase(string caseName)
        {           
            Open_connection();
            SqlCommand cmd;
            string query = "SELECT caseID, userID, caseName FROM Cases WHERE caseName = @name";
            using (cmd = new SqlCommand(query, _cnn))
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
            UpdateDateAccessed(Case.caseID);
            _cnn.Close();
        }
            
        //Gets the status of the case 0 or 1 (closed or open).
        public int GetCaseStatus(string caseName)
        {
            Open_connection();
            int status = 0;
            SqlCommand cmd;
            string query = "SELECT caseStatus FROM Cases WHERE caseName = @name";
            using (cmd=new SqlCommand(query, _cnn))
            {
                cmd.Parameters.AddWithValue("@name", caseName);
                status = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

            }
            return status;
        }

        //Used for the end report.
        public List<String> GetReport()
        {
            List<String> data = new List<string>();
            byte[] hash = new byte[64];
            int thisCase = Case.caseID;
            Open_connection();
            SqlCommand cmd;
            string query = "SELECT captureDate, sourceLink, eviDesc, filePath, indecentFlag, fileHash FROM Evidence WHERE caseID = @caseID";
            using (cmd = new SqlCommand(query, _cnn))
            {
                cmd.Parameters.AddWithValue("@caseID", thisCase);
                SqlDataReader reading = cmd.ExecuteReader();
                while (reading.Read())
                {
                    data.Add(reading["captureDate"].ToString());
                    data.Add(reading["sourceLink"].ToString());
                    data.Add(reading["eviDesc"].ToString());
                    string fp = reading["filePath"].ToString();
                    data.Add(fp);
                    data.Add(reading["indecentFlag"].ToString());

                    hash = (byte[])reading["fileHash"];
                    
                    var hexString = BitConverter.ToString(hash);
                    hexString = hexString.Replace("-", "");
                    Hashing h = new Hashing();
                    (string hashResults, string objHash) = h.CheckHashes(hexString, fp);
                    data.Add(hexString);
                    data.Add(objHash);
                    data.Add(hashResults);
                }                
            }
            cmd.Dispose();
            return data;
        }

        //private string ConvertHash(byte[] data)
        //{
        //    char[] chars = data.Select(b => (char)b).ToArray();
        //    return new string(chars);
        //}
        
        //Updates case status to closed.
        public void CloseCase()
        {
            int close = 0;
            int caseID = Case.caseID;
            Open_connection();
            SqlCommand cmd;
            string query = "UPDATE dbo.Cases SET caseStatus = @close WHERE caseID = @caseID";
            using (cmd = new SqlCommand(query, _cnn))
            {
                cmd.Parameters.AddWithValue("@close", close);
                cmd.Parameters.AddWithValue("@caseID", caseID);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            _cnn.Close();
        }
    }
}
