using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OSINTBrowser
{
    public class Sherlock
    {

        private bool _Finished = false;
        private string _SearchForThis = "";
        public string _searchForThis
        {
            get { return _SearchForThis; }
            set { _SearchForThis = value; }
        }

        public bool _finished
        {
            get { return _Finished; }
            set { _Finished = value; }
        }

        //Starts the process to run Sherlock.
        public bool LaunchSherlock(string sherlockSearchTerm)
        {
            _searchForThis = sherlockSearchTerm;
            try
            {
                //Sets up the bits for a new process
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe");
                myProcessStartInfo.CreateNoWindow = true;
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.Arguments = "/r C:\\Users\\saral\\source\\repos\\sherlock\\sherlock\\sherlock.py -fo " + Case.CaseFilePath + " " + _searchForThis;
                myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process myProcess = new Process();
                myProcess.StartInfo = myProcessStartInfo;

                //myProcess.WaitForExit();
                myProcess.Start();
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += (a, b) =>
                {
                    MessageBox.Show("Sherlock scan finished");
                    ProcessFinished();

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

        public void ProcessFinished()
        {
            _finished = true;
        }

        //Converts Sherlock .txt file into HTML to display.
        private string DisplayHTML(string search)
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
                        sb.AppendLine($"<a href='{str}' target='_blank'>{str}</a></br>");
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

        public string MakeNewTab(string thisSearch)
        {
            string htmlResults = DisplayHTML(thisSearch);
            return htmlResults;
        }
    }
}



