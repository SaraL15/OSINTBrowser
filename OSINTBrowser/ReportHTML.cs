using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSINTBrowser
{
    internal class ReportHTML
    {
        //When button report is clicked - get all information from the case folder, and display in HTML links.

 
        public string GetFiles()
        {

            string path = Case.CaseFilePath;

            string[] fileEntries = Directory.GetFiles(path);
            var sb = new StringBuilder();

            foreach (string fileName in fileEntries)
            {
                var sr = new StringReader(fileName);
                var str = sr.ReadLine();
                while (str != null)
                {
                    str = str.TrimEnd();
                    str.Replace("  ", " &nbsp;");
                    if (str.Length > 80)
                    {
                        sb.AppendLine($"<p>{str}</p>");
                    }
                    else if (str.Length > 0)
                    {
                        sb.AppendLine($"<a href=''>{str}</a></br>");
                    }
                    str = sr.ReadLine();
                }
            }                    
            
            return sb.ToString();

        }
    }
    
}
