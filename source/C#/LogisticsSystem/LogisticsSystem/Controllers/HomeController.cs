using System;
using System.Web.Mvc;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            Session[Define.Session.CONTROLLER] = RouteData.Values[Define.Session.CONTROLLER];
            Session[Define.Session.ACTION] = RouteData.Values[Define.Session.ACTION];
            return View();
        }
        public ContentResult ErrorMsg(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return Content("Error");
            }
            return Content(ErrorPack.GetError(Server.MapPath("~/ErrorCode/ErrorCode.xml"), code, (LanguageType?)Session["languageType"]));
        }
        /// <summary>
        /// 익스플로러 8이하 일경우 경고 에러 페이지
        /// </summary>
        /// <returns></returns>
        public ActionResult BrowserError()
        {
            Session[Define.Session.CONTROLLER] = RouteData.Values[Define.Session.CONTROLLER];
            Session[Define.Session.ACTION] = RouteData.Values[Define.Session.ACTION];
            return View();
        }
    }
}
