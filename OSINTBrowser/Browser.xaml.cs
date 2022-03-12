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

    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>

    public partial class Browser : Window
    {
        TabItem currentTabItem = null;
        ChromiumWebBrowser currentBrowser = null;

        
        int tabCount = 0;
        public string currentUrl { get { return txtAddressBar.Text; } }
        public string SherlockSearchTerm
        {
            get { return txtSearchBox.Text; }
            set { txtSearchBox.Text = value; }
        }
 
        string queryUrl = "https://duckduckgo.com/?q=";
        Sherlock s = new Sherlock();
        Capture c = null;


        public Browser()
        {
            InitializeComponent();
            loadBookmarks();
        }

        //Opens a new tab containing a browser.
        private void btnNewTab_Click(object sender, RoutedEventArgs e)
        {
            TabItem newTab = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            tabControl.Items.Add(newTab);
            tabCount++;

            newTab.Content = browser;
            browser.Address = "https://www.google.com";

            newTab.Header = "New Tab";

            currentBrowser = browser;
            currentTabItem = newTab;
            browser.Loaded += FinishedLoadingWebpage;
        }

        //public void htmlTab(string resultHtml)
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


        //Tries to put name of the site on the tab **TODO - needs to be fixed as it shows massive urls still :D**
        private void FinishedLoadingWebpage(object sender, RoutedEventArgs e)
        {
            var sndr = sender as ChromiumWebBrowser;
            if (currentTabItem != null)
            {
                string removeHttp = sndr.Address.Replace("http://www.", "");
                string removeHttps = removeHttp.Replace("https://www.", "");

                currentTabItem.Header = removeHttps;
            }
            currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
        }

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
            currentBrowser.Reload();
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

       
        //Enter key event for txtAddressBar for url navigation
        private void txtAddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Go();
            }
        }

        //Turn this into a search bar on the browser - be able to select different search engines.
        private void Search()
        {
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
        }

        private void Go()
        {
            currentBrowser.Load(txtAddressBar.Text);
            currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            string folder = Case.CaseFilePath;
            using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
            {
                string date = DateTime.Now.ToString();
                sw.WriteLine(date + ": Site Visited: " + txtAddressBar.Text, "/n");
            }


        }

        //Changes the text within the txtAddressBar to show current url.
        private void CurrentBrowser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            txtAddressBar.Text = currentBrowser.Address;
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

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            Go();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }


        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThis = new Screenshot();
            captureThis.screenCapture(currentUrl);
        }



        private void btnSnip_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThisSnip = new Screensnip();
            captureThisSnip.captureType = "Screen Snip";
            captureThisSnip.screenCapture(currentUrl);
        }

        private void ChangeSearch_Click(object sender, RoutedEventArgs e)
        {
            var selectSearch = sender as FrameworkElement;
            if (selectSearch != null)
            {
               // ChangeSearch.ContextMenu.IsOpen = true;
            }
        }
        
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
            //Probably set as a default rather than google.
            queryUrl = "https://duckduckgo.com/?q=";
        }

        //Yandex
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //Russian but has a good image search.
            queryUrl = "https://yandex.com/search/?text=";
        }

        private Sherlock getSherlockInstance()
        {
            
            return s;
        }
        private void Sherlock_Click(object sender, RoutedEventArgs e)
        {
            string searchTermSherlock = txtSearchBox.Text;
            bool success = false;
            try
            {
                Sherlock s = getSherlockInstance();
                s.launchSherlock(searchTermSherlock);
                string folder = Case.CaseFilePath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(folder, "Log.txt"), true))
                {
                    string date = DateTime.Now.ToString();
                    sw.WriteLine(date + ": Sherlock Search: " + searchTermSherlock, "/n");
                }
                success = s.processFinished();
                while (success != true)
                {
       
                    btnSherlock.Visibility = Visibility.Visible;
                }
                btnTest.Visibility = Visibility.Visible;
                btnSherlock.Visibility = Visibility.Hidden;

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

        //public void Sherlock_Exist()
        //{
        //    Sherlock sherlock = new Sherlock();
        //    string sherlockedName = sherlock.searchForThis;
        //    string filePathString = @":\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\bin\Debug\" + sherlockedName + ".txt";
        //    if (!File.Exists(filePathString))
        //    {
        //        //Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => btnTest.Visibility = Visibility.Visible));
        //        //btnTest.Visibility = Visibility.Visible;
        //        //Thread thread = new Thread(() => showBtn());
        //        //thread.SetApartmentState(ApartmentState.STA);
        //        //thread.Start();
        //        btnTest.Visibility = Visibility.Visible;
                
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        //public void showBtn()
        //{
        //    //btnTest.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action( () => btnTest.Visibility = Visibility.Visible));
        //    //this.Dispatcher.Invoke(() =>
        //    //{

        //    //    btnTest.Visibility = Visibility.Visible;
        //    //});
            
        //}

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {


                Sherlock s = getSherlockInstance();
                string thisSearch = s.searchForThis;
                string resultHtml = s.makeNewTab(thisSearch);

                TabItem nt = new TabItem();
                ChromiumWebBrowser b = new ChromiumWebBrowser();
                tabControl.Items.Add(nt);
                nt.Content = b;
                nt.Header = "Results";


                currentBrowser = b;
                currentTabItem = nt;
                b.LoadHtml(resultHtml, "http://results/");
                b.MouseLeftButtonDown += B_MouseLeftButtonDown;
            
        }

        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TabItem newTab = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            tabControl.Items.Add(newTab);
            tabCount++;

            newTab.Content = browser;
            browser.Address = "https://www.google.com";

            newTab.Header = "New Tab";

            currentBrowser = browser;
            currentTabItem = newTab;
            browser.Loaded += FinishedLoadingWebpage;
        }

        //private void menuItemBookmarks()
        //{
        //    MenuItem menuItem1 = new MenuItem();
        //    menuItem1.Header = "Test 123";
        //    this.mnuBookmark.Items.Add(menuItem1);
        //}

        private void loadBookmarks()
        { 
            MenuItem bookmarks = new MenuItem();
            bookmarks.Header = "facebook";
            mnuBookmark.Items.Add(bookmarks);

            MenuItem openMenuItem = new MenuItem();
            bookmarks.Items.Add(openMenuItem);
            openMenuItem.Header = "Open";
            openMenuItem.Click += OpenMenuItem_Click;
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            c = new Record();
            c.captureType = "Record";
            btnRecord.Visibility = Visibility.Hidden;
            btnStop.Visibility = Visibility.Visible;
            c.screenCapture(currentUrl);

            
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnRecord.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Hidden;
            stop_Recording(c as Record);
            
            System.Drawing.Bitmap bmp = (Bitmap)System.Drawing.Image.FromFile(@"C:\Users\saral\OneDrive\Desktop\Misc\2022_03_02_Testernson");
            CaptureWindow cpw = new CaptureWindow(txtAddressBar.Text);
            cpw.showScreenshot(bmp, 3);
            cpw.Topmost = true;
            cpw.Show();

        }

        private void stop_Recording (Record c)
        {
            c.EndRecording();

        }
    }
}
