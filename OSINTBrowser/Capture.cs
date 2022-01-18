using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OSINTBrowser
{
    public class Capture
    {
        private string CaptureName;
        private string CaptureType;

        public string captureName
        {
            get { return CaptureName; }
            set { CaptureName = value; }
        }

        public string captureType
        {
            get { return CaptureType; }
            set { CaptureType = value; }
        }

        public Capture() { }
        public Capture(string captureName, string capturetype)
        { 
            CaptureName = captureName; 
            CaptureType = capturetype; 
        }

       
        public void screenShot()
        {
            string captureType = "Screenshot";
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            SaveCapture(bmpScreenshot, captureType);
            
        }

        //Save location of the capture.
        //**TODO** file name will be casename_date_capturetypeX
        private void SaveCapture(Bitmap bmp, string captureType)
        {
            DateTime dateTime = DateTime.Now.ToUniversalTime();
            string captureDate = DateTime.Now.ToString("yyMMddHHmmss");
            string captureName = "";
            string captureLocation = "";
            //string splitpath = new DirectoryInfo(Case.CaseFilePath).Name;
            //splitpath = splitpath.Substring(12);
            
            //string saveCaptureName = captureDate + "_" + splitpath + "_" + captureType;
            SaveFileDialog saveDlog = new SaveFileDialog();
            saveDlog.InitialDirectory = Case.CaseFilePath;
            saveDlog.FileName = "screenshot.png";
            captureName = saveDlog.FileName;
            captureLocation = saveDlog.InitialDirectory;
            saveDlog.Title = "Save Capture";
            saveDlog.Filter = "PNG File | *.png";
            ImageFormat format = ImageFormat.Png;
            if (saveDlog.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveDlog.FileName);
                //logCapture(saveCaptureName);

            }

            DbConnect dbc = new DbConnect();
            dbc.open_connection();
            dbc.save_to_database(dateTime, captureName, captureLocation);
        }

        private void logCapture(string captureName)
        {
            using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(Case.CaseFilePath, "Log.txt"), true))
            {
                sw.WriteLine("Capture taken: " + captureName, "/n");
            }
        }
    }
}