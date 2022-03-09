using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OSINTBrowser
{
    public class Hashing
    {
        public string GetHash(HashAlgorithm hashAlgo, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgo.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var builder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString());
            }

            // Return the hexadecimal string.
            return builder.ToString();
        }

        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        // Verify a hash against a string.
        //  private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        //   {
        // Hash the input.
        //var hashOfInput = GetHash(hashAlgorithm, input);

        // Create a StringComparer an compare the hashes.
        //    StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        // return comparer.Compare(hashOfInput, hash) == 0;
        // }
    }
    // The example displays the following output:
    //    The SHA256 hash of Hello World! is: 7f83b1657ff1fc53b92dc18148a1d65dfc2d4b1fa3d677284addd200126d9069.
    //    Verifying the hash...
    //    The hashes are the same.



}

