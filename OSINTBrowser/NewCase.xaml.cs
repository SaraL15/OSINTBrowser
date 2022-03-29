using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;



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
            Directory.CreateDirectory("OSIB_Cases");
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

        private bool checkFolderIsValid(string folder)
        {
            bool valid = false;
            if (!Directory.Exists(folder))
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Please check folder path is valid");
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                valid = true;
            }
            return valid;
        }

        private void create_new_case(string folder)
        {

            //Gets the input from the form and creates a new folder.
            if (validateUserInput() == false)
            {
                System.Windows.Forms.MessageBox.Show("Name or description missing");
            }
            else if (validateUserInput() == true) { 

                string subjectName = txtSubject.Text;
                string description = txtDesc.Text;
                DateTime now = DateTime.Now;
                string creationDate = DateTime.Now.ToString("yyyy_MM_dd");
                string subfolder = creationDate + "_" + subjectName;
            
                string pathString = Path.Combine(folder, subfolder);
                if (Directory.Exists(pathString))
                {
                    Console.WriteLine("Folder \"{0}\" already exists.", subfolder);
                    lblError.Content = "Folder already exists";
                }
                else
                {
                    Directory.CreateDirectory(pathString);
                    lblError.Content = "";

                    //Class attributes are static so they can be accessed 
                    Case.CaseName = subjectName;
                    //Case.CaseUser = Environment.UserName;
                    //string investigator = Case.CaseUser;
                    Case.CaseCreationDate = creationDate;
                    Case.CaseFilePath = folder;
                    //Creates a new log file and inputs the data from the newCase object.
                    string fileName = "Log.txt";
                    string filepathString = Path.Combine(pathString, fileName);

                    Console.WriteLine("Path to my file is {0}\n", pathString);

                    if (!File.Exists(filepathString))
                    {
                        using (StreamWriter sw = new StreamWriter(filepathString))
                        {
                            string[] logLines =
                            {
                            "Case Name: " + Case.CaseName + "  Case opened: " + Case.CaseCreationDate
                        };
                            foreach (string l in logLines)
                            {
                                sw.WriteLine(l);
                            }
                        }
                    }
                    DbConnect dbc = new DbConnect();
                    //dbc.create_new_table();
                    dbc.addNewCase(now, subjectName, description);

                    Case.CaseFilePath = pathString;
                    
                    string lastFolderName = Path.GetFileName(pathString);
                    string folderName = lastFolderName.Substring(11);
                    dbc.getTheCase(folderName);
                    Browser browser = new Browser();
                    browser.Show();
                    this.Close();
                }
            }
        }

        //Check file path is valid.
        private void validateFilePath()
        {
            string selectedFolder = txtFolderPath.Text;
            if (checkFolderIsValid(selectedFolder) == false)
            {
                browseForFolder();
            }
            else
            {
                create_new_case(selectedFolder);
            }
        }

        //Check textboxes are not empty.
        private bool validateUserInput()
        {
            string subject = txtSubject.Text;
            string description = txtDesc.Text;

            if (!string.IsNullOrEmpty(subject) && (!string.IsNullOrEmpty(description)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private void btnCreate_Click(object sender, EventArgs e)
        {
            validateFilePath();
        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
            
        }

    }
}

