using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace OSINTBrowser
{
    public class Hashing
    {
        //Convert image to bytes for the hash to go in the database.
        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            //var data = Encoding.UTF8.GetBytes(img.ToString());
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public string checkHashes(byte[] dbHash, string file)
        {
            file = file + ".png";
            //Byte[] data = ImageToByte(img);
            Byte[] hashResult;
            using (SHA512 shaM = new SHA512Managed())
            {
                //hashResult = shaM.ComputeHash(data);
                using (FileStream fs = File.OpenRead(file))
                {
                   hashResult = shaM.ComputeHash(fs);
                }
                     
            }

            if (hashResult.SequenceEqual(dbHash))
            {
                return "Hashes match";
            }
            return "Hashes do not match - Image may have been edited!";
        }
    }
}

