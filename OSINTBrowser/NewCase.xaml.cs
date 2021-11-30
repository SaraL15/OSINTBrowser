using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace OSINTBrowser
{
    /// <summary>
    /// Interaction logic for NewCase.xaml
    /// </summary>
    public partial class NewCase : Window
    {
        public NewCase()
        {
            InitializeComponent();
        }

        private void btnFolderPath_Click(object sender, RoutedEventArgs e)
        {
            browseForFolder();
        }

        private void browseForFolder()
        {
            //Opens the Windows Browser Explorer to select where to save the newly...
            //...created folder.

            string selectedFolder = "";
            //Opens the folder browser
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = true;
            DialogResult result = folder.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                //selectedFolder will hold the folder path.
                Console.WriteLine(folder.SelectedPath);
                selectedFolder = folder.SelectedPath;
            }
            else
            {
                lblError.Content = "Please select a folder to save your case into.";
            }
            txtFolderPath.Text = selectedFolder;
        }


        private void btnCreate_Click(object sender, EventArgs e)
        {

            //Gets the input from the form and creates a new folder.

            string name = txtSubject.Text;
            string user = txtUser.Text;
            string organisation = txtOrganisation.Text;
            string creationDate = DateTime.Now.ToString("yyyy_MM_dd");
            string folder = txtFolderPath.Text;


            DateTime lastAccessed = DateTime.Now;

            string subfolder = creationDate + "_" + name;
            string pathString = System.IO.Path.Combine(folder, subfolder);
            if (System.IO.Directory.Exists(pathString))
            {
                Console.WriteLine("Folder \"{0}\" already exists.", subfolder);
                lblError.Content = "Folder already exists";
            }
            else
            {
                System.IO.Directory.CreateDirectory(pathString);
                lblError.Content = "";
                //New Case object.
                // Case newCase = new Case(name, user, creationDate, organisation, pathString);
                Case.CaseName = name;
                Case.CaseUser = user;
                Case.CaseOrganisationName = organisation;
                Case.CaseCreationDate = creationDate;   
                Case.CaseFilePath = folder;
                //Creates a new log file and inputs the data from the newCase object.
                string fileName = "Log.txt";
                string filepathString = System.IO.Path.Combine(pathString, fileName);

                Console.WriteLine("Path to my file is {0}\n", pathString);

                if (!System.IO.File.Exists(filepathString))
                {
                    using (StreamWriter sw = new StreamWriter(filepathString))
                    {
                        string[] logLines =
                        {
                            Case.CaseCreationDate, "Case Name: " + Case.CaseName, "Investigator Name: " + Case.CaseUser,
                            "Organisation Name: " + Case.CaseOrganisationName
                        };
                        foreach (string l in logLines)
                        {
                            sw.WriteLine(l);
                        }
                    }
                }
                Browser browser = new Browser();
                browser.Show();
                this.Close();
                

                //**CLOSES WINDOW AND GOES TO MAIN BROWSER**
            }


        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

