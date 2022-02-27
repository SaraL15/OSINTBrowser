using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;


namespace OSINTBrowser
{
 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNewCase_Click(object sender, RoutedEventArgs e)
        {
            NewCase makeNewCase = new NewCase();
            makeNewCase.Show();
            this.Close();
        }

        private void btnOpenCase_Click(object sender, RoutedEventArgs e)
        {
            //Opens up Windows folder browser and allows to select existing folder.
            string selectedFolder = "";
            //Opens the folder browser
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = true;
            var result = folder.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                //selectedFolder will hold the folder path.
                Console.WriteLine("Sucessfully Opened " + folder.SelectedPath);
                selectedFolder = folder.SelectedPath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(selectedFolder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine("Case last accessed " + date, "/n");
                }            
                getCaseDetails(selectedFolder);
                
            }
            else
            {
                return;
            }
        }

        private void getCaseDetails(string mySelectedFolder)
        {
            if (mySelectedFolder == "")
            {
                return ;
            }
            else
            {
                Case.CaseFilePath = mySelectedFolder;
                DbConnect dbc = new DbConnect();
                string lastFolderName = Path.GetFileName(mySelectedFolder);
                string folderName = lastFolderName.Substring(11);
                dbc.getTheCase(folderName);
                openBrowser();
            }
        }

        private void openBrowser()
        {
            Browser bw = new Browser();
            bw.Show();
            this.Close();
        }
        
    }
}
