using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OSINTBrowser
{
    public class Sherlock
    {

       
        private string SearchForThis = "";
        public string searchForThis
        {
            get { return SearchForThis; }
            set { SearchForThis = value; }
        }
        public void launchSherlock(string sherlockSearchTerm)
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
                myProcessStartInfo.Arguments = "/r C:\\Users\\saral\\source\\repos\\sherlock\\sherlock\\sherlock.py " + searchForThis;
                myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process myProcess = new Process();
                myProcess.StartInfo = myProcessStartInfo;
                myProcess.Start();
                myProcess.WaitForExit();
                Console.WriteLine("Sherlock Finished");


                //string displayThis = displayHTML(searchForThis);
                //makeNewTab(displayThis);

                //DialogResult result = System.Windows.Forms.MessageBox.Show("Sherlock scan complete");
                //if (result == DialogResult.OK)
                //{
                //    //Thread thread = new Thread(() => displayCompleted());
                //    //thread.SetApartmentState(ApartmentState.STA);
                //    //thread.Start();
                    
                //}

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Sherlock could not launch, try again later.");


            }


        }

        //private void sherlockFinished(object sender, EventArgs e)
        //{

        //    DialogResult result = System.Windows.Forms.MessageBox.Show("Sherlock scan complete");
        //    if (result == DialogResult.OK)
        //    {
        //        //Thread thread = new Thread(() => displayCompleted());
        //        //thread.SetApartmentState(ApartmentState.STA);
        //        //thread.Start();
        //        displayCompleted();
        //    }


        //    //displayCompleted();
        //}

        //private void displayCompleted()
        //{
        //    Browser b = new Browser();
        //    b.Sherlock_Exist();
        //    //Thread thread = new Thread(() => b.Sherlock_Exist());
        //    //thread.SetApartmentState(ApartmentState.STA);
        //    //thread.Start();
        //    //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        //    //{
        //    //    b.Sherlock_Exist();
        //    //}));

        //    //System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate()
        //    //{
        //    //    Browser b = new Browser();
        //    //    b.Sherlock_Exist();
        //    //});

        //    //System.Windows.Application.Current.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => b.Sherlock_Exist());

        //    //Browser b = new Browser();


        //    //b.Sherlock_Exist();


        //}

        //public void Sherlock_Exist()
        //{
        //    //Sherlock sherlock = new Sherlock();
        //    string sherlockedName = searchForThis;
        //    string filePathString = @":\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\bin\Debug\" + sherlockedName + ".txt";
        //    if (!File.Exists(filePathString))
        //    {
        //        //Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => btnTest.Visibility = Visibility.Visible));

        //        //Thread thread = new Thread(() => showBtn());
        //        //thread.SetApartmentState(ApartmentState.STA);
        //        //thread.Start();
        //        //b.showBtn();


        //        //Thread thread = new Thread(() => b.showBtn());
        //        //thread.SetApartmentState(ApartmentState.STA);
        //        //thread.Start();
        //        //b.btnTest.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new btnTestVisability(CheckButtonVisability));

        //        //Thread thread = new Thread(delegate ()
        //        //{
        //        //    b.showBtn();
        //        //});
        //        //thread.IsBackground = true;
        //        //thread.Start();
        //        Browser b = new Browser();
        //        b.showBtn();
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        private string displayHTML(string search)
        {
            string path = @"C:\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\bin\Debug\" + search + ".txt";
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

        public string makeNewTab(string thisSearch)
        {
            string htmlResults = displayHTML(thisSearch);

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



