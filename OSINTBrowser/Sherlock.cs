using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OSINTBrowser
{
    public class Sherlock
    {

        private bool Finished = false;
        private string SearchForThis = "";
        public string searchForThis
        {
            get { return SearchForThis; }
            set { SearchForThis = value; }
        }

        public bool finished
        {
            get { return Finished; }
            set { Finished = value; }
        }

        public bool launchSherlock(string sherlockSearchTerm)
        {
            //string FileName = "cmd.exe";
            //string Arguments = "/c C:\\Users\\saral\\source\\repos\\sherlock\\sherlock\\sherlock.py " + searchForThis;
            searchForThis = sherlockSearchTerm;
            try
            {
                //Sets up the bits for a new process
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe");
                myProcessStartInfo.CreateNoWindow = true;
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.Arguments = "/r C:\\Users\\saral\\source\\repos\\sherlock\\sherlock\\sherlock.py -fo " + Case.CaseFilePath + " " + searchForThis;
                myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process myProcess = new Process();
                myProcess.StartInfo = myProcessStartInfo;

                //myProcess.WaitForExit();
                myProcess.Start();
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += (a, b) =>
                {
                    MessageBox.Show("Sherlock scan finished");
                    processFinished();

                };
                
                Console.WriteLine("Sherlock Finished");

                return true;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sherlock could not launch, try again later.");
                return false;

            }


        }

        public void processFinished()
        {
            finished = true;
        }

        private string displayHTML(string search)
        {
            try
            {
                string path = Case.CaseFilePath + "\\" + search + ".txt";
                string readText = File.ReadAllText(path);

                var sb = new StringBuilder();

                var sr = new StringReader(readText);
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

                return sb.ToString();
            }
            catch
            {
                return "Sherlock results return failed - possible empty field name. Try again :)";
            }

        }

        public string makeNewTab(string thisSearch)
        {
            string htmlResults = displayHTML(thisSearch);

            return htmlResults;
        }

    }
}



