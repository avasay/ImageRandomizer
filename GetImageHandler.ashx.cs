using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageRandomizer
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.ContentType = "image/jpeg";
            string rootDir = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            string userFolder = string.Empty;

            if (context.Request.QueryString["subject"] != null)
            {
                userFolder = Path.Combine(@"images\banners", context.Request.QueryString["subject"]);
            }
            else
            {
                userFolder = @"images\banners\default";
            }


            DirInfoCacher dirInfoCacher = new DirInfoCacher(Path.Combine(rootDir, userFolder));

            string pickedImage = dirInfoCacher.pickedImagePath;

            if (pickedImage == "")
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("");
            }
            else
            {
                ImageCacher imageCacher = new ImageCacher(pickedImage);

                byte[] byteArray = imageCacher.GetImage();
                context.Response.BinaryWrite(byteArray);
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}