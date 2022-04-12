using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;


namespace OSINTBrowser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Opens instance of the newCase window.
        private void btnNewCase_Click(object sender, RoutedEventArgs e)
        {
            NewCase makeNewCase = new NewCase();
            makeNewCase.Show();
            this.Close();
        }

        //Selecting which case to open.
        private void btnOpenCase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Opens up Windows folder browser and allows to select existing folder.
                string selectedFolder = "";
                FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.ShowNewFolderButton = true;
                var result = folder.ShowDialog();

                //Checks if result is empty.
                if (result.ToString() != string.Empty)
                {
                    //selectedFolder will hold the folder path.               
                    selectedFolder = folder.SelectedPath;

                    //checks to see if log.txt file exists - therefore a case has been created.
                    if (File.Exists(selectedFolder + "/Log.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(Path.Combine(selectedFolder, "Log.txt"), true))
                        {
                            string date = DateTime.Now.ToString();
                            sw.WriteLine("Case last accessed " + date, "/n");
                        }
                        Console.WriteLine("Sucessfully Opened " + folder.SelectedPath);
                        GetCaseDetails(selectedFolder);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Not an existing case, check selection.");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Case could not be opened, perhaps it has already been close?");
            }
        }

        //Fills out the case details.
        private void GetCaseDetails(string mySelectedFolder)
        {
            try
            {
                if (mySelectedFolder == "")
                {
                    return;
                }
                else
                {
                    //Uses database to get the case details.
                    Case.CaseFilePath = mySelectedFolder;
                    DbConnect dbc = new DbConnect();
                    string lastFolderName = Path.GetFileName(mySelectedFolder);
                    string folderName = lastFolderName.Substring(11);
                    if (dbc.GetCaseStatus(folderName) != 0)
                    {
                        dbc.GetTheCase(folderName);
                        OpenBrowser();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Case has been closed");
                    }        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Windows.Forms.MessageBox.Show("Failed to open case, check selection.");
                return;
            }           
        }

        //Opens browser object when case is valid and closes MainWindow.
        private void OpenBrowser()
        {
            Browser bw = new Browser();
            bw.Show();
            this.Close();
        }        
    }
}
