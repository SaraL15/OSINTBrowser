using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        private void newtabMenuItem_Click(object sender, RoutedEventArgs e)
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
                currentBrowser.Address = "https://www.google.com/search?q=" + txtSearchBox.Text;
                currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            }
        }

        private void Go()
        {
            currentBrowser.Load(txtAddressBar.Text);
            currentBrowser.AddressChanged += CurrentBrowser_AddressChanged;
            

        }

        private void CurrentBrowser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            txtAddressBar.Text = currentBrowser.Address;
        }

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
    }
}
