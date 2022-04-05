using System;
using System.Windows;


namespace OSINTBrowser
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private string _results;
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void btnCancelReport_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnProduceReport_Click(object sender, RoutedEventArgs e)
        {
            //Empty text boxes will be allowed.
            string caseDesc = txtDesc.Text;
            string caseComments = txtComment.Text;
            
            
            
            ReportHTML r = new ReportHTML();
            //Browser b = new Browser();
            try
            {
                _results = r.GetTheFiles(caseDesc, caseComments);
                //r.StartReport(caseDesc, caseComments);
                //b.displayReport(caseDesc, caseComments);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Unable to create report");
            }
        }

        public string thesefuckingresults()
        {
            return _results;
            
        }
    }
}
