using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OSINTBrowser
{
    internal class ReportHTML
    {
        //Get the file details from the Evidence database, convert to HTML and display.
        public string GetTheFiles(string desc, string comment)
        {
            StringBuilder sb = new StringBuilder();
            string results = "";
            string id = Case.caseID.ToString();
            string name = Case.caseName;
            List<string> dbOutput = new List<string>();
            List<string> headers = new List<string>();

            sb.AppendLine($"<h3>Case: {id} {name}</h3></br>");
            sb.AppendLine($"<h4>Description: </h4>{desc}</br>");
            sb.AppendLine($"<h4>Comment: </h4>{comment}</br>");
            
            headers.Add("<b>Date: </b>");
            headers.Add("<b>Source: </b>");
            headers.Add("<b>Description: </b>");
            headers.Add("<b>File Path: </b>");
            headers.Add("<b>Indicent Flag: </b>");
            headers.Add("<b>Hash from Database: </b>");
            headers.Add("<b>Hash from Original: </b>");
            headers.Add("<b>Match: </b>");

            DbConnect db = new DbConnect();
            dbOutput = db.GetReport();
            
            //i  is used to control the amount of lines.
            int i = 0;
            
            //dbOutput holds the results from the database.
            foreach (string output in dbOutput)
            {
                var sr = new StringReader(output);
                var str = sr.ReadLine();
                
                while (str != null)
                {
                    sb.Append(headers[i]);
                    str = str.TrimEnd();
                    str.Replace("  ", " &nbsp;");
                     
                    if (str.Contains("http") || i == 3)
                    {
                        sb.AppendLine($"<a href='{str}' target='_blank'>{str}</a></br>");
                    }
                    else if (i == 7)
                    {
                        if (str.Contains("do not"))
                        {
                            sb.AppendLine($"<b>{str}</b></br></br>");
                        }
                        sb.AppendLine($"<b>{str}</b></br></br>");
                    }
                    else
                    {
                        sb.AppendLine($"{str}</br>");
                    }
                    str = sr.ReadLine();

                    i++;
                    //Resets i to 0 to start the next loop for the next file.
                    if (i > 7)
                    {
                        i = 0;
                    }
                }
            }
            results = sb.ToString();
            return results;
        }
    }   
}
