using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;


namespace OSINTBrowser
{

    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>

    public partial class Browser : Window
    {
        TabItem currentTabItem = null;
        ChromiumWebBrowser currentBrowser = null;
        Sherlock s = null;
        Capture c = null;

        private bool firstClick = false;
        private int tabCount = 0;
        public string CurrentUrl { get { return txtAddressBar.Text; } }
        private string searchTermSherlock = null;
        private string queryUrl = "https://duckduckgo.com/?q=";



        public Browser()
        {
            InitializeComponent();
            // loadBookmarks();
        }

        //Webbrowser buttons and tabs.
        //Opens a new tab containing a browser.
        private void btnNewTab_Click(object sender, RoutedEventArgs e)
        {
            //If first time loading up Marple - enable buttons 
            if (firstClick == false)
            {
                EnableButtons();
                MakeNewTab();
                firstClick = true;
            }
            else if (firstClick == true)
            {
                MakeNewTab();
            }
        }

        private void MakeNewTab()
        {
            TabItem newTab = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
           
            browser.Name = "browser" + tabCount;
            tabControl.Items.Add(newTab);
            newTab.Name = "tab" + tabCount;
            tabCount++;
            newTab.Content = browser;
            browser.Address = "https://www.google.com";
            newTab.Focus();
            tabControl.SelectedItem = newTab;
            //Updates the browser and tab
            currentTabItem = newTab;
            currentBrowser = browser;
            browser.Loaded += FinishedLoadingWebpage;
        }

        private void EnableButtons()
        {
            txtAddressBar.IsEnabled = true;
            txtSearchBox.IsEnabled = true;
            btnBack.IsEnabled = true;
            btnFwd.IsEnabled = true;
            btnGo.IsEnabled = true;
            btnRefresh.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }

        //Tries to put name of the site on the tab
        private void FinishedLoadingWebpage(object sender, RoutedEventArgs e)
        {
            string removewww = "";
            var s = sender as ChromiumWebBrowser;
            //currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            txtAddressBar.Text = s.Address;
            if (currentTabItem != null)
            {
                string url = s.Address;
                string hosturl = GetUri(url);
                removewww = hosturl.Replace("www.", "");
                

                
            }
            currentTabItem.Header = removewww;

        }

        private string GetUri(string url)
        {
            var uri = new Uri(url);
            var thisuri = uri.Host;
            return thisuri;
        }


       

        //Changes the selected tab and browser to the current tab and browser.
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null)
            {
                currentTabItem = tabControl.SelectedItem as TabItem;
            }

            if (currentTabItem != null)
            {
                currentBrowser = currentTabItem.Content as ChromiumWebBrowser;
            }
        }


        //Turn this into a search bar on the browser - able to select different search engines.
        private void Search()
        {
            {
                if (!string.IsNullOrWhiteSpace(txtSearchBox.Text))
                {
                    currentBrowser.Address = queryUrl + txtSearchBox.Text;
                    currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
                    string folder = Case.CaseFilePath;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
                    {
                        string date = DateTime.Now.ToString();
                        sw.WriteLine(date + ": Search Term: " + txtSearchBox.Text + " " + queryUrl, "/n");
                    }
                }
                else
                {
                    Console.WriteLine("White space!");
                }

            }
        }

        private void Go()
        {
            if (!string.IsNullOrWhiteSpace(txtAddressBar.Text))
            {
                currentBrowser.Load(txtAddressBar.Text);
                currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
                currentBrowser.Loaded += FinishedLoadingWebpage;

                //currentTabItem.Header = txtAddressBar.Text;


                string folder = Case.CaseFilePath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine(date + ": Site Visited: " + txtAddressBar.Text, "/n");
                }
            }
            else
            {
                Console.WriteLine("White space!");
            }


        }

        //Changes the text within the txtAddressBar to show current url.
        private void CurrentBrowser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            txtAddressBar.Text = e.NewValue.ToString();
            //txtAddressBar.Text = currentBrowser.Address;
        }


        //Search bar keydown on enter event.
        private void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void btnCloseTab_Click(object sender, RoutedEventArgs e)
        {
            if (tabCount > 0 && currentTabItem != null)
            {
                tabControl.Items.Remove(currentTabItem);
            }
        }

        //Some Sherlock functionality which works with the browser.
        private void btnSherlock_Click(object sender, RoutedEventArgs e)
        {
            searchTermSherlock = txtSearchBox.Text;
            var ts = new ThreadStart(SherlockScan);
            
            var backgroundThread = new Thread(ts);
            
            backgroundThread.Start();
            btnSherlockResults.Visibility = Visibility.Visible;
        }

        private void SherlockScan()
        {
            try
            {
                bool success = false;
                s = new Sherlock();
                success = s.launchSherlock(searchTermSherlock);
                string folder = Case.CaseFilePath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine(date + ": Sherlock Search: " + searchTermSherlock, "/n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string getSherlockSearchTerm()
        {
            string searchTerm = txtSearchBox.Text;
            return searchTerm;
        }

        private void btnSherlockResults_Click(object sender, RoutedEventArgs e)
        {

                string thisSearch = s.searchForThis;
                string resultHtml = s.makeNewTab(thisSearch);

                TabItem nt = new TabItem();
                ChromiumWebBrowser b = new ChromiumWebBrowser();
                tabControl.Items.Add(nt);
                nt.Content = b;
                nt.Header = "Sherlock";


                currentBrowser = b;
                currentTabItem = nt;

                b.LoadHtml(resultHtml, "http://sherlockresults/");
            
                //b.MouseLeftButtonDown += B_MouseLeftButtonDown;
            
        }

        //private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    TabItem newTab = new TabItem();
        //    ChromiumWebBrowser browser = new ChromiumWebBrowser();
        //    tabControl.Items.Add(newTab);
        //    tabCount++;

        //    newTab.Content = browser;
        //    browser.Address = "https://www.google.com";

        //    newTab.Header = "New Tab";

        //    currentBrowser = browser;
        //    currentTabItem = newTab;
        //    browser.Loaded += FinishedLoadingWebpage;
        //}

        //Capture buttons.
        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThis = new Screenshot();
            captureThis.ScreenCapture(CurrentUrl);
        }

        private void btnSnip_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThisSnip = new Screensnip();
            captureThisSnip.captureType = "Screen Snip";
            captureThisSnip.ScreenCapture(CurrentUrl);
        }

        //Screen record functionality.
        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            c = new Record();
            c.captureType = "Record";
            btnRecord.Visibility = Visibility.Hidden;
            btnStop.Visibility = Visibility.Visible;
            c.ScreenCapture(CurrentUrl);           
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnRecord.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Hidden;
            stop_Recording(c as Record);
            
            Bitmap bmp = (Bitmap)System.Drawing.Image.FromFile(@"C:\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\Resources\rec_placeholder.bmp");
            CaptureWindow cpw = new CaptureWindow(txtAddressBar.Text);
            cpw.ShowScreenshot(bmp, 3);
            cpw.Topmost = true;
            cpw.Show();
        }

        private void stop_Recording (Record c)
        {
            c.EndRecording();
        }

        ReportWindow rw = new ReportWindow();
        //Possible to pop up in a new window?
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            
            //ReportWindow rw = new ReportWindow();
            rw.Show();
            btnReport.Visibility = Visibility.Hidden;
            btnShowReport.Visibility = Visibility.Visible;
            
        }

        public void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            //ReportHTML r = new ReportHTML();
            //string resultHtml = r.th;
            //r.GetTheFiles();
            //MessageBox.Show("Report created.");

            string rr = rw.ReportResults();
            ;
            TabItem nt = new TabItem();
            ChromiumWebBrowser cb = new ChromiumWebBrowser();
            tabControl.Items.Add(nt);
            nt.Content = cb;
            nt.Header = "Report";


            currentBrowser = cb;
            currentTabItem = nt;

            cb.LoadHtml(rr, "http://report");
        }



        //Search Engine selection.
        //Google
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            queryUrl = "https://www.google.com/search?q=";
        }

        //Bing
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            queryUrl = "https://www.bing.com/search?q=";
        }

        //DuckDuckGo
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //Set as a default search.
            queryUrl = "https://duckduckgo.com/?q=";
        }

        //Yandex
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //Russian but has a good image search.
            queryUrl = "https://yandex.com/search/?text=";
        }



        //Browser and Button Control.
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (currentBrowser.CanGoBack)
            {
                currentBrowser.Back();
            }
        }

        private void btnFwd_Click(object sender, RoutedEventArgs e)
        {
            if (currentBrowser.CanGoForward)
            {
                currentBrowser.Forward();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentBrowser.Reload();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            Go();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        //Enter key event for txtAddressBar for url navigation
        private void txtAddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Go();
            }
        }
    }
}
