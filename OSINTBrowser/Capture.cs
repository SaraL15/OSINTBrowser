using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using ScreenRecorderLib;
namespace OSINTBrowser
{
    /*Capture is an abstract class. Screenshot, Screensnip and Video are inherited from it.
     Capture contains methods to log and save captures to selected locations.
     Also saves filepath into database.
    */
    public abstract class Capture
    {
        public string captureType { get; set; }       
        public abstract void ScreenCapture(string source);

        //Logs the capture within Log.txt
        public void LogCapture(string captureDate, string captureName, string captureDesc, string captureSource)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Case.CaseFilePath, "Log.txt"), true))
            {
                sw.WriteLine(captureDate + ": Capture: " + captureSource + " " + captureDesc + " " + captureName, "/n");
            }
        }

        //Saves the capture in the Case folder and the database. A hash of the file is created and also saved into database.
        public void SaveCapture(Image bmp, string description, string source, bool? check)
        {            
            DateTime dateTime = DateTime.Now.ToUniversalTime();
            string dateForLog = dateTime.ToString();
            string captureDate = dateTime.ToString("yyMMddHHmmss");
            string captureName = "capture" + captureDate;

            SaveFileDialog saveDlog = new SaveFileDialog();
            saveDlog.InitialDirectory = Case.CaseFilePath;
            saveDlog.FileName = captureName;

            string captureSaveLocation = saveDlog.InitialDirectory + @"\" + captureName;
            saveDlog.Title = "Save Capture";
            saveDlog.Filter = "PNG File | *.png";
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;

            if (saveDlog.ShowDialog() == DialogResult.OK)
            {
                captureName = saveDlog.FileName;
                bmp.Save(saveDlog.FileName);
                LogCapture(dateForLog, captureName, description, source);
            }

            //Get the hash of the file to store into the database.
            Hashing h = new Hashing();
            Byte[] data = h.ImageToByte(bmp);
            Byte[] hashResult;
            using (SHA512 shaM = new SHA512Managed())
            {
                hashResult = shaM.ComputeHash(data);
            }

            //Open database connection and save
            DbConnect dbc = new DbConnect();
            dbc.Open_connection();
            dbc.CaptureToDatabase(dateTime, description, source, captureSaveLocation, check, hashResult);
        }
    }

    //Screenshot class, inherited from Capture
    public class Screenshot : Capture
    {      
        //Screenshot - **currently only the primary display.**
        public override void ScreenCapture(string source)
        {
            captureType = "Screenshot";
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);                     
            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            //Opens CaptureWindow for user to add some more infomation.
            CaptureWindow cpw = new CaptureWindow(source);
            cpw.ShowScreenshot(bmpScreenshot, 1);
            cpw.Topmost = true;
            cpw.Show();
        }
    }

    //Snipping class, inherited from Capture
    public class Screensnip : Capture
    {
        private Rectangle canvasBounds = Screen.GetBounds(Point.Empty);
        public override void ScreenCapture(string source)
        {
            try
            {
                SetCanvas();
                Console.WriteLine(canvasBounds.Width + " " + canvasBounds.Height);
                var bmpScreenshot = new Bitmap(canvasBounds.Width, canvasBounds.Height, PixelFormat.Format32bppArgb);
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(canvasBounds.Left, canvasBounds.Top, 0, 0, bmpScreenshot.Size);
                gfxScreenshot.Save();

                CaptureWindow cpw = new CaptureWindow(source);
                cpw.ShowScreenshot(bmpScreenshot, 2);
                cpw.Topmost = true;
                cpw.Show();
            }
            catch
            {
                return;
            }

        }

        //Sets the 'overlay' canvas for the screensnip
        public void SetCanvas()
        {
            using (Canvas canvas = new Canvas())
            {
                if (canvas.ShowDialog() == DialogResult.OK)
                {
                    canvasBounds = canvas.GetRectangle();
                }
            }
        }
    }

    //Record class, inherited from Capture
    public class Record : Capture
    {
        Recorder rec;
        string videoPath = "";
        string path = "";

        //Entry method. Calls createRecording and LogCapture
        public override void ScreenCapture(string source)
        {
            string dateTime = DateTime.Now.ToString();
            try
            {               
                CreateRecording(dateTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
            LogCapture(dateTime, videoPath, "Capture recording", source);
        }

        //Records the main display with audio.
        public void CreateRecording(string dateTime)
        {
            try
            {               
                rec = Recorder.CreateRecorder(new RecorderOptions
                {
                    AudioOptions = new AudioOptions
                    {
                        IsAudioEnabled = true,
                    }
                }); 
                rec.OnRecordingComplete += Rec_OnRecordingComplete;
                rec.OnRecordingFailed += Rec_OnRecordingFailed;
                rec.OnStatusChanged += Rec_OnStatusChanged;
                //Record to a file
                videoPath = Path.Combine(Case.CaseFilePath, dateTime);
                rec.Record(videoPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Failed to record, try again." + ex.Message);
                Console.WriteLine(ex.Message);
            }            
        }
        public void EndRecording()
        {
            rec.Stop();
        }

        //From ScreenRecorderLib documentation.
        private void Rec_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            //Get the file path if recorded to a file
            path = e.FilePath;
        }
        private void Rec_OnRecordingFailed(object sender, RecordingFailedEventArgs e)
        {
            string error = e.Error;
        }
        private void Rec_OnStatusChanged(object sender, RecordingStatusEventArgs e)
        {
            RecorderStatus status = e.Status;
        }

        //Saves the recording to the database
        public void SaveRecording(string description, string source, bool? check)
        {
            DateTime dateTime = DateTime.Now.ToUniversalTime();
            string captureSaveLocation = Case.CaseFilePath;

            //Gets the hash value of the mp4 file to save into the database.
            Byte[] hashResult;
            using (SHA512 shaM = new SHA512Managed())
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    hashResult = shaM.ComputeHash(fs);
                }
            }

            //Open database connection and save
            DbConnect dbc = new DbConnect();
            dbc.Open_connection();
            dbc.CaptureToDatabase(dateTime, description, source, path, check, hashResult);
        }
    }
}