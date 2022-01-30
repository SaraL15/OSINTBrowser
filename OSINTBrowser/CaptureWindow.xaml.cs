using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace OSINTBrowser
{
    /// <summary>
    /// Interaction logic for CaptureWindow.xaml
    /// </summary>
    public partial class CaptureWindow : Window
    {
        Bitmap saveThisImage = null;
        int captureType = 0;
        public CaptureWindow()
        {
            InitializeComponent();
        }

  
        public void showScreenshot(Bitmap image, int type)
        {
            AddToPreview(image);
            captureType = type;
        }

        //Converts the bitmap into a bitmap image which can be used in the preview image paine.
        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
         
        }
        private void AddToPreview(Bitmap snap)
        {
            saveThisImage = snap;
            BitmapImage thisImage = BitmapToImageSource(snap);
            image.Source = thisImage;
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            string desc = txtDescription.Text;
            string source = txtSource.Text;
            var check = chkIndecent.IsChecked;

            if (captureType == 1)
            {           
                Capture c = new Screenshot();
                c.saveCapture(saveThisImage, desc, source, check);
                this.Close();
            }

            if (captureType == 2)
            {
                Capture s = new Screensnip();
                s.saveCapture(saveThisImage, desc, source, check);
                this.Close();
            }


        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

