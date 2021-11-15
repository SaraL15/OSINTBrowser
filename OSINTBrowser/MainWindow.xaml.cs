using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            //this.Hide();
            makeNewCase.Show();
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
                using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(selectedFolder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine("Case last accessed " + date, "/n");
                }

                Browser bw = new Browser();
                bw.Show();
            }
            else
            {
                return;
            }
        }
    }
}
