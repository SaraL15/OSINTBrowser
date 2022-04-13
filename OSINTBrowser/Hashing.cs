using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace OSINTBrowser
{
    public class Hashing
    {
        //Convert image to bytes for the hash to go in the database.
        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        //Comparing the hash stored in the database to the hash of the file.
        public (string matchResults, string objectHash) CheckHashes(string dbHash, string file)
        {
            string fileType = Path.GetFileName(file);

            if (fileType.StartsWith("capture"))
            {
                file = file + ".png";
            }

            //Getting the hash of the saved file.
            Byte[] hashResult;
            using (SHA512 shaM = new SHA512Managed())
            {
                using (FileStream fs = File.OpenRead(file))
                {
                   hashResult = shaM.ComputeHash(fs);
                }                     
            }
            var hexString = BitConverter.ToString(hashResult);
            hexString = hexString.Replace("-", "");

            //Comparing the hashes.
            if (hexString == dbHash)
            {
                return ("Hashes match", hexString);
            }
            return ("*****Hashes do not match - Image may have been edited!*****", hexString);
        }
    }
}

