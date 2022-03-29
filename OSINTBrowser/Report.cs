using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
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
        public void CreatePdf()
        {
            int i = 0;
            int n = 0;
            List<Image> images = new List<Image>();

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            gfx.DrawString("Hello world", font, XBrushes.Black,
                new XRect(20,20, page.Width, page.Height),
                XStringFormats.Center);

            foreach (FileInfo captureImage in new DirectoryInfo("C:\\Users\\saral\\OneDrive\\Desktop\\OSIB\\2022_03_13_CreateNew_Test\\images").GetFiles())
            {
                images.Add(Image.FromFile(captureImage.FullName));

                Image img = images[i];
                MemoryStream stream = new MemoryStream();
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                //Continue creating PDF
                XImage xImg = XImage.FromStream(stream);
                gfx.DrawImage(xImg, n, 0);
                img.Dispose();

                i = i ++;
                n = n + 100;
            }



            string imagesample = "C:\\Users\\saral\\OneDrive\\Desktop\\OSIB\\2022_03_13_CreateNew_Test\\capture220313151642.png";
            //XImage image = XImage.FromFile(imagesample);
            //gfx.DrawImage(image, 40, 40);

            string filename = "helloworld.pdf";
            document.Save(Case.CaseFilePath + "\\" + filename);




        }
    }
}
