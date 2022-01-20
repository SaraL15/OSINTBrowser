using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace OSINTBrowser
{
    /*Capture is an abstract class. Screenshot, Screensnip and Video are inherited from it.
     Capture contains methods to log and save captures to selected locations **Perhaps I will change so there is no option for location and goes into case file directly.**
     Also saves filepath into database.
    */
    public abstract class Capture
    {
        public abstract void screenCapture();

        public void logCapture(string captureName)
        {
            using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(Case.CaseFilePath, "Log.txt"), true))
            {
                sw.WriteLine("Capture taken: " + captureName, "/n");
            }
        }

 
        //**TODO** file name will be casename_date_capturetypeX
        public void saveCapture(Bitmap bmp, string captureType)
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
            //Open database connection and save.
            DbConnect dbc = new DbConnect();
            dbc.open_connection();
            dbc.save_to_database(dateTime, captureName, captureLocation);
        }


    }

    public class Screenshot : Capture
    { 
        //Screenshots - currently only the primary display.
        public override void screenCapture()
        {
            string captureType = "Screenshot";
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            saveCapture(bmpScreenshot, captureType);
            
        }   
    }

    public class Screensnip : Capture
    {
        private Rectangle canvasBounds = Screen.GetBounds(Point.Empty);
        public override void screenCapture()
        {
            
            string captureType = "Screensnip";
            
            SetCanvas();
            //GetSnapShot();
           // Rectangle selection = canvasBounds;
            var bmpScreenshot = new Bitmap(canvasBounds.Width, canvasBounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(canvasBounds.Left, canvasBounds.Top, 0, 0, bmpScreenshot.Size, CopyPixelOperation.SourceCopy);
            saveCapture(bmpScreenshot, captureType);


        }

        //public Bitmap GetSnapShot()
        //{
        //    using (Image image = new Bitmap(canvasBounds.Width, canvasBounds.Height))
        //    {
        //        using (Graphics graphics = Graphics.FromImage(image))
        //        {
        //            graphics.CopyFromScreen(new Point
        //            (canvasBounds.Left, canvasBounds.Top), Point.Empty, canvasBounds.Size);
        //        }
               
        //        return new Bitmap(SetBorder(image, Color.Black, 1));
        //    }
        //}

        //private Image SetBorder(Image srcImg, Color color, int width)
        //{
        //    // Create a copy of the image and graphics context
        //    Image dstImg = srcImg.Clone() as Image;
        //    Graphics g = Graphics.FromImage(dstImg);

        //    // Create the pen
        //    Pen pBorder = new Pen(color, width)
        //    {
        //        Alignment = PenAlignment.Center
        //    };

        //    // Draw
        //    g.DrawRectangle(pBorder, 0, 0, dstImg.Width - 1, dstImg.Height - 1);

        //    // Clean up
        //    pBorder.Dispose();
        //    g.Save();
        //    g.Dispose();

        //    // Return
        //    return dstImg;
        //}

        public void SetCanvas()
        {
            using (Canvas canvas = new Canvas())
            {
               
                if (canvas.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.canvasBounds = canvas.GetRectangle();
                }
            }
        }

    }

    public class Record : Capture
    {
        public override void screenCapture()
        {
        }
    }

}