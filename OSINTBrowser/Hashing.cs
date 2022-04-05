using System.Drawing;

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
    }
}

