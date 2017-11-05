using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.IO;

namespace LogisticsSystem.Controllers
{
    public class AJAXController : AbstractController
    {
        [AuthorizeFilter]
        public ActionResult Imageupload(HttpPostedFileBase file)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return Content("NG");
            }
            if (file == null || file.ContentLength <= 0)
            {
                return Content("NG");
            }

            using (Stream st = file.InputStream)
            {
                byte[] pData = new byte[st.Length];
                st.Read(pData, 0, pData.Length);
                Session[Define.Session.IMAGE] = pData;
            }
            return Content("OK");
        }
        [AuthorizeFilter]
        public ActionResult ImageBuffer()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return Content("NG");
            }
            byte[] buffer = (byte[])Session[Define.Session.IMAGE];
            Session.Remove(Define.Session.IMAGE);
            if (buffer != null)
            {
                MemoryStream pStream = new MemoryStream(buffer);
                return File(pStream, "image/jpg");
            }
            return Content("NG");
        }
    }
}
