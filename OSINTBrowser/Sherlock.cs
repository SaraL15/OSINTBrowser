using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace OSINTBrowser
{
   public class Sherlock
    {
        private string displayThis;
        string searchForThis = "fquin";

        public void launchSherlock()
        {
            
            try
            {
                //Sets up the bits for a new process
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe");
                myProcessStartInfo.CreateNoWindow = true;
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.Arguments = "/r C:\\Users\\saral\\source\\repos\\sherlock\\sherlock\\sherlock.py " + searchForThis;
                myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process myProcess = new Process();
                myProcess.StartInfo = myProcessStartInfo;
                myProcess.Start();
                myProcess.WaitForExit();
                Console.WriteLine("Sherlock Finished");
                
                //string displayThis = displayHTML(searchForThis);
                //makeNewTab(displayThis);               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sherlock could not launch, try again later.");
            }

        }

        private string displayHTML(string search)
        {
            string path = @"C:\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\bin\Debug\" + search +".txt";
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

        public string makeNewTab ()
        {
            string htmlResults = displayHTML(searchForThis);

            return htmlResults;
        }

    }


            //Process p = new Process();
            //p.StartInfo.FileName = System.IO.Path.Combine(@"C:\Users\saral\source\repos\sherlock\sherlock\sherlock.py");
            //p.StartInfo.Arguments =  "/c testaccount1";
            ////p.StartInfo.WorkingDirectory = @"C:\Users\saral\source\repos\sherlock\sherlock";
            //p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.Start();
            //p.WaitForExit();

            //Process process = new Process();
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/c python3 sherlock testaccount1";
            //startInfo.WorkingDirectory = @"C:\\Users\\saral\\source\\repos\\sherlock";
            //process.StartInfo = startInfo;
            //process.Start();

            //ProcessStartInfo p = new ProcessStartInfo();
            //p.Arguments = "python3 sherlock fiddlequinn";
            //p.FileName = (@"C:\Users\saral\AppData\Local\Programs\Python\Python39\python.exe");
            //p.UseShellExecute = false;
            //p.WorkingDirectory = (@"C:\Users\saral\source\repos\sherlock\sherlock\");
            //p.CreateNoWindow = false;
            //p.RedirectStandardOutput = true;

            //Process process = Process.Start(p);
            //Console.WriteLine("complete?");



}



