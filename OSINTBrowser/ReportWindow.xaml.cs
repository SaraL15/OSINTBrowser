using System;
using System.Windows;

namespace OSINTBrowser
{
    public partial class ReportWindow : Window
    {
        private string _results;
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void BtnCancelReport_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnProduceReport_Click(object sender, RoutedEventArgs e)
        {
            //Empty text boxes will be allowed.
            string caseDesc = txtDesc.Text;
            string caseComments = txtComment.Text;
            var checkForClose = chkCloseCase.IsChecked;

            ReportHTML r = new ReportHTML();
            //Browser b = new Browser();
            try
            {
                _results = r.GetTheFiles(caseDesc, caseComments);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Unable to create report");
            }

            try 
            {
                if (checkForClose == true)
                {
                    DbConnect db = new DbConnect();
                    db.CloseCase();
                    MessageBox.Show("Case is now closed");
                    this.Close();
                    //close the browser.
                }
            }
            catch
            {
                Console.WriteLine("Could not close the case");
            }
        }
        public string ReportResults()
        {
            return _results;        
        }
    }
}
