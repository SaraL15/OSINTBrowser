using System.Collections.Generic;
using System.IO;

namespace OSINTBrowser
{
    internal class Bookmarks
    {
        private Dictionary<string, string> bookmarks = new Dictionary<string, string>();

        private string GetBookmarks(string site)
        {
            TextFileToDictionary();
            site = bookmarks[site];
            return site;
        }   

        public string SelectedBookmark(string getSite)
        {
            getSite = GetBookmarks(getSite);
            return getSite;
        }

        private void TextFileToDictionary()
        {
            //Hard coded file path.
            using (var sr = new StreamReader(@"C:\Users\saral\source\repos\OSINTBrowser\OSINTBrowser\Resources\Bookmarks.txt"))
            {
                string line = null;
                // while it reads a key
                while ((line = sr.ReadLine()) != null)
                {
                    // add the dictionary key and site
                    bookmarks.Add(line, sr.ReadLine());
                }
            }
        }
    }
}
