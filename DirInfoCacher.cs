using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageRandomizer
{
    public class DirInfoCacher
    {
        public string pickedImagePath { get; set; }

        public DirInfoCacher(string path)
        {
            pickedImagePath = PickImage(path);
        }

        private string PickImage(string path)
        {
            string pickedImage = string.Empty;
            object cacheList = HttpRuntime.Cache.Get(path) as List<string>;

            if (cacheList == null)
            {
                pickedImage = BuildCache(path);
            }
            else
            {
                pickedImage = PickFromCache(cacheList);
            }
            return pickedImage;
        }

        private string BuildCache(string path)
        {
            // Get a list of files from the given path
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                return "";
            }

            List<System.IO.FileInfo> fileInfoList = dirInfo.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower())).ToList();

            if (fileInfoList.Count() != 0)
            {
                // Pick random file
                Random R = new Random();
                string imagePath = string.Empty;
                int randomNumber = R.Next(0, fileInfoList.Count());
                imagePath = fileInfoList.ElementAt(randomNumber).FullName;

                // Now, put all files in the Dictionary
                List<string> fileInfo2string = fileInfoList.Select(f => f.FullName.ToString()).ToList();
                HttpRuntime.Cache.Insert(path, fileInfo2string, null, DateTime.Now.AddMinutes(60d), System.Web.Caching.Cache.NoSlidingExpiration);
                return imagePath;
            }
            else
            {
                return "";
            }
        }

        private string PickFromCache(object cacheList)
        {
            IList<string> collection = (IList<string>)cacheList;

            // Pick random file
            Random R = new Random();
            string imagePath = string.Empty;
            int randomNumber = R.Next(0, collection.Count());
            imagePath = collection.ElementAt(randomNumber);
            return imagePath;
        }
    }
}