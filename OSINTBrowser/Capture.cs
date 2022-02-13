using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;



namespace OSINTBrowser
{
    /*Capture is an abstract class. Screenshot, Screensnip and Video are inherited from it.
     Capture contains methods to log and save captures to selected locations **Perhaps I will change so there is no option for location and goes into case file directly.**
     Also saves filepath into database.
    */
    public abstract class Capture
    {
        public string captureType { get; set; }
        

        public abstract void screenCapture(string source);

        public void logCapture(string captureDate, string captureName, string captureDesc, string captureSource)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Case.CaseFilePath, "Log.txt"), true))
            {
                sw.WriteLine(captureDate + ": Capture: " + captureSource + " " + captureDesc + " " + captureName, "/n");
            }
        }

        public void saveCapture(Image bmp, string description, string source, bool? check)
        {
            
            DateTime dateTime = DateTime.Now.ToUniversalTime();
            string dateForLog = dateTime.ToString();
            string captureDate = dateTime.ToString("yyMMddHHmmss");
            string captureName = "capture" + captureDate;
            //string saveCaptureName = "";

            SaveFileDialog saveDlog = new SaveFileDialog();
            saveDlog.InitialDirectory = Case.CaseFilePath;
            saveDlog.FileName = captureName;

            string captureSaveLocation = saveDlog.InitialDirectory;
            saveDlog.Title = "Save Capture";
            saveDlog.Filter = "PNG File | *.png";
            ImageFormat format = ImageFormat.Png;


            //bmp.Save(saveDlog.FileName);
            //logCapture(captureDate, captureName, description, source);
            //MessageBox.Show("Capture Saved in Case Folder");

            if (saveDlog.ShowDialog() == DialogResult.OK)
            {
                captureName = saveDlog.FileName;
                bmp.Save(saveDlog.FileName);

                //saveCaptureName = new DirectoryInfo(captureName).Name;
                logCapture(dateForLog, captureName, description, source);
                

            }

            //Get the hash of the file to store into the database.
            Hashing h = new Hashing();
            Byte[] data = h.ImageToByte(bmp);
            Byte[] result;
            SHA512 shaM = new SHA512Managed();
            result = shaM.ComputeHash(data);


            //Open database connection and save
            DbConnect dbc = new DbConnect();
            dbc.open_connection();
            dbc.captureToDatabase(dateTime, description, source, captureSaveLocation, check, result);



        }
    }

    //Take a Screenshot
    public class Screenshot : Capture
    {

        //Screenshots - currently only the primary display.
        public override void screenCapture(string source)
        {
            captureType = "Screenshot";
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            //saveCapture(bmpScreenshot);
            CaptureWindow cpw = new CaptureWindow(source);
            cpw.showScreenshot(bmpScreenshot, 1);
            cpw.Topmost = true;
            cpw.Show();


            //saveCapture(bmpScreenshot, captureType);

        }


    }

    //Take a snip
    public class Screensnip : Capture
    {
        private Rectangle canvasBounds = Screen.GetBounds(Point.Empty);
        public override void screenCapture(string source)
        {
            //string desc = "test";
            //string source = "test1";
            //bool check = false;

            setCanvas();
            Console.WriteLine(canvasBounds.Width + " " + canvasBounds.Height);
            var bmpScreenshot = new Bitmap(canvasBounds.Width, canvasBounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(canvasBounds.Left, canvasBounds.Top, 0, 0, bmpScreenshot.Size);

            gfxScreenshot.Save();
            //saveCapture(bmpScreenshot, desc, source, check);

            CaptureWindow cpw = new CaptureWindow(source);
            cpw.showScreenshot(bmpScreenshot, 2);
            cpw.Topmost = true;
            cpw.Show();


            //Bitmap snipped = new Bitmap(bmpScreenshot);
            //Rectangle snippedRect = new Rectangle(0, 0, 100, 100);
            //Bitmap snippedBitmap = snipped.Clone(snippedRect, snipped.PixelFormat);



        }

        //Sets the 'overlay' canvas for the screensnip
        public void setCanvas()
        {
            using (Canvas canvas = new Canvas())
            {
                if (canvas.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    canvasBounds = canvas.GetRectangle();
                }
            }

        }
    }

    public class Record : Capture
    {
        public override void screenCapture(string source)
        {

        }
    }
}