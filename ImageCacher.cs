using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageRandomizer
{
    public class ImageCacher
    {
        private string m_pickedImagePath = string.Empty;
        private ImageObject m_imageObject = null;

        public ImageCacher(string pickedImage)
        {
            m_pickedImagePath = pickedImage;
            m_imageObject = HttpRuntime.Cache.Get(m_pickedImagePath) as ImageObject;
        }

        public byte[] GetImage()
        {
            if (m_imageObject == null)
            {              
                DateTime ourFileDate = File.GetLastWriteTime(m_pickedImagePath);
                ourFileDate = ourFileDate.AddMilliseconds(-ourFileDate.Millisecond);

                byte[] byteArray = File.ReadAllBytes(m_pickedImagePath);
                m_imageObject = new ImageObject(m_pickedImagePath, "image/jpeg", byteArray, ourFileDate);
                HttpRuntime.Cache.Insert(m_pickedImagePath, m_imageObject, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);               
            }
                      
            return m_imageObject.Content;          
        }

    }

    public class ImageObject
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime SubmitDate { get; set; }

        public ImageObject(string fn, string tp, byte[] ct, DateTime dt)
        {
            FileName = fn;
            ContentType = tp;
            Content = ct;
            SubmitDate = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, DateTimeKind.Utc);
        }

    }


}