using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;


namespace OSINTBrowser
{
    internal class Report
    {
        /*Create a PDF of all information gathered.
         * Show screenshots and snips - with their respective source links and file names
         * Show recording capture name - with file path
         * Filepath to log
         * 
         * */

        public void StartReport(string desc, string comments)
        {
            CreatePdf(desc, comments);

        }



        private void CreatePdf(string desc, string comments)
        {
            string titleName = Case.CaseName;
            int i = 0;
            int n = 0;
            int startX = 100;
            int startY = 300;
            List<Image> images = new List<Image>();

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);

            XFont titleFont = new XFont("Verdana", 30, XFontStyle.Bold);
            XFont font = new XFont("Verdana", 18, XFontStyle.Regular);
            var rect = new XRect(100, 300, 400, 100);

            gfx.DrawString(titleName, titleFont, XBrushes.Black, new XPoint(200, 70));
            gfx.DrawLine(XPens.Black, new XPoint(100, 100), new XPoint(500, 100));


            int l = desc.Length + 10;
            tf.DrawString(desc, font,  XBrushes.Black, rect);
            tf.DrawString(comments, font, XBrushes.Black, rect);

            //gfx.DrawLine(XPens.Blue, 0, 0, 30, 30);

            //foreach (FileInfo captureImage in new DirectoryInfo(@"C:\Users\saral\OneDrive\Desktop\OSIB\2022_04_01_moogie").GetFiles())
            //{
            //    Image img = images[i];
            //    MemoryStream stream = new MemoryStream();
            //    if (captureImage.Name.StartsWith("capture"))
            //    {
            //        images.Add(Image.FromFile(captureImage.FullName));

                    
                    
            //        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            //    }
                

            //    //Continue creating PDF
            //    XImage xImg = XImage.FromStream(stream);
            //    gfx.DrawImage(xImg, n, 0);
            //    img.Dispose();

            //    i = i ++;
            //    n = n + 100;
            //}



            string imagesample = "C:\\Users\\saral\\OneDrive\\Desktop\\OSIB\\2022_03_13_CreateNew_Test\\capture220313151642.png";
            //XImage image = XImage.FromFile(imagesample);
            //gfx.DrawImage(image, 40, 40);

            string filename = "helloworld.pdf";
            document.Save(Case.CaseFilePath + "\\" + filename);




        }
    }
}
