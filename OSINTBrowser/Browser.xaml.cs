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
      
        
        public Browser()
        {
            InitializeComponent();
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

        //Turn this into a search bar on the browser - be able to select different search engines. **TODO - add different search engines to select**
        private void Search()
        {
            {
                currentBrowser.Address = "https://www.google.com/search?q=" + txtSearchBox.Text;
                currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            }
        }

        private void Go()
        {
            currentBrowser.Load(txtAddressBar.Text);
            currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
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

        //Full screenshot of the current display - goes to Capture class.
        //private void screenshotMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    Capture captureThis = new Screenshot();

        //    captureThis.screenCapture();
        //}

        //private void snipMenuItem_Click_1(object sender, RoutedEventArgs e)
        //{
        //    Capture captureThis = new Screensnip();
        //    captureThis.captureType = "Screen Snip";
        //    captureThis.screenCapture();
        //}

        //private void recordMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    Capture captureThis = new Record();
        //    captureThis.captureType = "Recording";
        //    captureThis.screenCapture();

        //}

        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThis = new Screenshot();
            captureThis.screenCapture();
        }



        private void btnSnip_Click(object sender, RoutedEventArgs e)
        {
            Capture captureThisSnip = new Screensnip();
            captureThisSnip.captureType = "Screen Snip";
            captureThisSnip.screenCapture();
        }
    }
}
