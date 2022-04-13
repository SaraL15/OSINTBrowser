using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;

namespace OSINTBrowser
{
    public partial class Browser : Window
    {
        TabItem _currentTabItem = null;
        ChromiumWebBrowser _currentBrowser = null;
        Sherlock _sher = null;
        Capture _cap = null;

        private bool _firstClick = false;
        private int _tabCount = 0;
        public string CurrentUrl { get { return txtAddressBar.Text; } }
        private string _searchTermSherlock = null;
        private string _queryUrl = "https://duckduckgo.com/?q=";

        public Browser()
        {
            InitializeComponent();
        }

        //Webbrowser buttons and tabs.
        //Opens a new tab containing a browser.
        private void BtnNewTab_Click(object sender, RoutedEventArgs e)
        {
            //If first time loading up Marple - enable buttons. 
            if (_firstClick == false)
            {
                EnableButtons();
                MakeNewTab();
                _firstClick = true;
            }
            else if (_firstClick == true)
            {
                MakeNewTab();
            }
        }

        //For making a new tabitem.
        private void MakeNewTab()
        {
            TabItem newTab = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            browser.LifeSpanHandler = new CustomLifeSpanHandler();
            browser.Name = "browser" + _tabCount;
            tabControl.Items.Add(newTab);
            newTab.Name = "tab" + _tabCount;
            _tabCount++;
            newTab.Content = browser;
            browser.Address = "https://www.google.com";
            newTab.Focus();
            tabControl.SelectedItem = newTab;
            //Updates the browser and tab.
            _currentTabItem = newTab;
            _currentBrowser = browser;
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

        //Tries to put name of the site on the tab.
        private void FinishedLoadingWebpage(object sender, RoutedEventArgs e)
        {
            ForTab(sender);
            //string removeWww = "";
            //var s = sender as ChromiumWebBrowser;
            //_currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            //txtAddressBar.Text = s.Address;
            //if (_currentTabItem != null)
            //{
            //    string url = s.Address;
            //    string hostUrl = GetUri(url);
            //    removeWww = hostUrl.Replace("www.", "");
                
            //}           
            //_currentTabItem.Header = removeWww;
        }

        private string ForTab(object sender)
        {
            string removeWww = "";
            var s = sender as ChromiumWebBrowser;
            _currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            txtAddressBar.Text = s.Address;
            if (_currentTabItem != null)
            {
                string url = s.Address;
                string hostUrl = GetUri(url);
                removeWww = hostUrl.Replace("www.", "");

            }
            _currentTabItem.Header = removeWww;
            return removeWww;
        }

        private string GetUri(string url)
        {
            var uri = new Uri(url);
            var thisUri = uri.Host;
            return thisUri;
        }

        //Changes the selected tab and browser to the current tab and browser.
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null)
            {
                _currentTabItem = tabControl.SelectedItem as TabItem;
            }
            if (_currentTabItem != null)
            {
                _currentBrowser = _currentTabItem.Content as ChromiumWebBrowser;
            }
        }

        //Turns this into a search bar on the browser - able to select different search engines.
        private void Search()
        {
            if (!string.IsNullOrWhiteSpace(txtSearchBox.Text))
            {
                _currentBrowser.Address = _queryUrl + txtSearchBox.Text;
                _currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
                string folder = Case.CaseFilePath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine(date + ": Search Term: " + txtSearchBox.Text + " " + _queryUrl, "/n");
                }
            }
            else
            {
                Console.WriteLine("White space!");
            }            
        }

        //Loads the address.
        private void Go()
        {
            if (!string.IsNullOrWhiteSpace(txtAddressBar.Text))
            {
                _currentBrowser.Load(txtAddressBar.Text);
                _currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
                _currentBrowser.Loaded += FinishedLoadingWebpage;

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
            _currentTabItem.Header = ForTab(sender);
        }

        //Search bar keydown on enter event.
        private void TxtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void BtnCloseTab_Click(object sender, RoutedEventArgs e)
        {
            if (_tabCount > 0 && _currentTabItem != null)
            {
                tabControl.Items.Remove(_currentTabItem);
            }
        }

        //Some Sherlock functionality which works with the browser.
        private void BtnSherlock_Click(object sender, RoutedEventArgs e)
        {
            _searchTermSherlock = txtSearchBox.Text;
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
                _sher = new Sherlock();
                success = _sher.LaunchSherlock(_searchTermSherlock);
                string folder = Case.CaseFilePath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine(date + ": Sherlock Search: " + _searchTermSherlock, "/n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string GetSherlockSearchTerm()
        {
            string searchTerm = txtSearchBox.Text;
            return searchTerm;
        }

        private void BtnSherlockResults_Click(object sender, RoutedEventArgs e)
        {
                string thisSearch = _sher._searchForThis;
                string resultHtml = _sher.MakeNewTab(thisSearch);

                TabItem nt = new TabItem();
                ChromiumWebBrowser b = new ChromiumWebBrowser();
                tabControl.Items.Add(nt);
                nt.Content = b;
                nt.Header = "Sherlock";

                _currentBrowser = b;
                _currentTabItem = nt;
                b.LoadHtml(resultHtml, @"http://sherlockresults/");
            
                b.MouseLeftButtonDown += B_MouseLeftButtonDown;
            
        }

        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TabItem newTab = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            tabControl.Items.Add(newTab);
            _tabCount++;
            newTab.Content = browser;
            browser.Address = "https://www.google.com";
            newTab.Header = "New Tab";
            _currentBrowser = browser;
            _currentTabItem = newTab;
            browser.Loaded += FinishedLoadingWebpage;
        }

        //Capture buttons.
        private void BtnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThis = new Screenshot();
            captureThis.ScreenCapture(CurrentUrl);
        }

        private void BtnSnip_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThisSnip = new Screensnip();
            captureThisSnip.captureType = "Screen Snip";
            captureThisSnip.ScreenCapture(CurrentUrl);
        }

        //Screen record functionality.
        private void BtnRecord_Click(object sender, RoutedEventArgs e)
        {
            _cap = new Record();
            _cap.captureType = "Record";
            btnRecord.Visibility = Visibility.Hidden;
            btnStop.Visibility = Visibility.Visible;
            _cap.ScreenCapture(CurrentUrl);           
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            btnRecord.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Hidden;
            StopRecording(_cap as Record);
            
            Bitmap bmp = (Bitmap)System.Drawing.Image.FromFile(@"C:\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\Resources\rec_placeholder.bmp");
            CaptureWindow cpw = new CaptureWindow(txtAddressBar.Text);
            cpw.ShowRecord(bmp, 3, _cap as Record);
            cpw.Topmost = true;
            cpw.Show();
        }

        private void StopRecording (Record c)
        {
            c.EndRecording();
        }

        ReportWindow _rw = new ReportWindow();
        //Maybe make a pop up in a new window?
        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            _rw.Show();
            btnReport.Visibility = Visibility.Hidden;
            btnShowReport.Visibility = Visibility.Visible;    
        }

        public void BtnShowReport_Click(object sender, RoutedEventArgs e)
        {
            string rr = _rw.ReportResults();
            TabItem nt = new TabItem();
            ChromiumWebBrowser cb = new ChromiumWebBrowser();
            tabControl.Items.Add(nt);
            nt.Content = cb;
            nt.Header = "Report";

            _currentBrowser = cb;
            _currentTabItem = nt;
            cb.MouseLeftButtonDown += B_MouseLeftButtonDown;

            try
            {
                cb.LoadHtml(rr, "http://report");
            }
            catch
            {
                MessageBox.Show("Whoops, something went wrong!");
            }
            
        }

        //Search Engine selection.
        //Google
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            _queryUrl = "https://www.google.com/search?q=";
        }

        //Bing
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _queryUrl = "https://www.bing.com/search?q=";
        }

        //DuckDuckGo
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //Set as a default search.
            _queryUrl = "https://duckduckgo.com/?q=";
        }

        //Yandex
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //Russian but has a good image search.
            _queryUrl = "https://yandex.com/search/?text=";
        }

        //Browser and Button Control.
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBrowser.CanGoBack)
            {
                _currentBrowser.Back();
            }
        }

        private void BtnFwd_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBrowser.CanGoForward)
            {
                _currentBrowser.Forward();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentBrowser.Reload();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            Go();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        //Enter key event for txtAddressBar for url navigation
        private void TxtAddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Go();
            }
        }

        //Bookmark Menu Click.
        private void MenuItem_Bookmarks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var thing = sender as MenuItem;
                string siteName = thing.Name;
                Bookmarks bk = new Bookmarks();
                string goHere = bk.SelectedBookmark(siteName);
                if (_currentBrowser != null)
                {
                    _currentBrowser.Load(goHere);
                    _currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
                    _currentBrowser.Loaded += FinishedLoadingWebpage;
                }
                else
                {
                    MessageBox.Show("Please click on the + to begin");
                }
            }
            catch
            {
                Console.WriteLine("Error in opening a bookmark");
            }    
        }
    }
}
