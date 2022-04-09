using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OSINTBrowser
{
    internal class ReportHTML
    {
        //StringBuilder sb = new StringBuilder();
        //string results = "";
        //When button report is clicked - get all information from the case folder, and display in HTML links.
        //StringBuilder sb = new StringBuilder();

        //public string StartReport(string desc, string comment)
        //{
        //    string id = Case.caseID.ToString();
        //    string name = Case.caseName;
        //    sb.AppendLine($"<h3>Case: {id} {name}</h3></br></br>");
        //    sb.AppendLine($"<h2>Description: {desc}</br>");
        //    sb.AppendLine($"<h2>Description: {comment}</br>");
        //    if (desc.Length > 50)
        //    {
        //        desc = desc.Insert(50, "</br>");
        //        sb.Append(desc);
        //    }
        //    GetTheFiles(desc, comment);
        //    return null;

        //}


 
        public string GetTheFiles(string desc, string comment)
        {
            StringBuilder sb = new StringBuilder();
            string results = "";
            string id = Case.caseID.ToString();
            string name = Case.caseName;
            sb.AppendLine($"<h3>Case: {id} {name}</h3></br></br>");
            sb.AppendLine($"<h4>Description: </h4>{desc}</br>");
            sb.AppendLine($"<h4>Comment: </h4>{comment}</br></br>");
            
            List<string> dbOutput = new List<string>();
            List<string> headers = new List<string>();
            headers.Add("<b>Date: </b>");
            headers.Add("<b>Source: </b>");
            headers.Add("<b>Description: </b>");
            headers.Add("<b>File Path: </b>");
            headers.Add("<b>Indicent Flag: </b>");
            headers.Add("<b>Hash: </b>");
            DbConnect db = new DbConnect();
            dbOutput = db.getReport();
            

            //var sb = new StringBuilder();
            int i = 0;
            
            
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
                        sb.AppendLine($"<a href='' target='_blank'>{str}</a></br>");
                    }
                    else if (i == 5)
                    {
                        sb.AppendLine($"{str}></br></br>");
                    }
                    else
                    {
                        sb.AppendLine($"{str}</br>");
                    }
                    str = sr.ReadLine();


                    i++;
                    if (i > 5)
                    {
                        i = 0;
                    }
                }
            }
            results = sb.ToString();


            return results;

        }
        //public string returnString()
        //{
        //    return results;
        //}
    }
    
}
