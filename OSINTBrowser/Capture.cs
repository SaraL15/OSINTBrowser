using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.IO;

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
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            //bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
            SaveImage(bmpScreenshot);
        }

        private void SaveImage(Bitmap bmp)
        {
            SaveFileDialog saveDlog = new SaveFileDialog();
            saveDlog.InitialDirectory = @"C:\";
            saveDlog.FileName = "screenshot.png";
            saveDlog.Title = "Save Screenshot";
            saveDlog.Filter = "PNG File | *.png";
            ImageFormat format = ImageFormat.Png;
            if (saveDlog.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveDlog.FileName);
            }
        }
    }
}