using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LogisticsSystem.Models;
using LogisticsSystem.App_Code;
using LogisticsSystem.Dao;
using LogisticsSystem.Master;

namespace LogisticsSystem.App_Code
{
    public abstract class AbstractController : Controller
    {
        protected UserInfo UserSession
        {
            get { return (UserInfo)Session[Define.Session.USER_INFO]; }
        }

        protected CompanyInfo CompanySession
        {
            get { return (CompanyInfo)Session[Define.Session.COMPANY_INFO]; }
        }

        protected bool CheckAuth()
        {
            return SessionCheck(Define.Session.AUTH_CHECK) && UserSession != null && CompanySession != null;
        }
        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            return Redirect(FormsAuthentication.LoginUrl);
        }
        public ActionResult ErrorPage(String strUrl)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            return Redirect(strUrl);
        }
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            String controller = (String)Session[Define.Session.CONTROLLER];
            String action = (String)Session[Define.Session.ACTION];
            Session[Define.Session.VIEW_NAME] = viewName;

            LanguageType? lType = GetLanguageType();
            Session[Define.Session.LANGUAGE_TYPE] = lType;

            if (controller != null && action != null)
            {
                ViewBag.Disp = new LanguagePack(Server.MapPath("~/Language"), controller, action, lType);
            }
            if (Define.MASTER_VIEW.Equals(masterName))
            {
                Session[Define.Session.MASTER] = masterName;
                ViewBag.Master = new LanguagePack(Server.MapPath("~/Language"), "./", "Master", lType);
                ViewBag.Navigate = new NavigationPack(Server.MapPath("~/Navigation"), controller, action, lType);
            }
            else
            {
                Session[Define.Session.MASTER] = null;
            }
            Session[Define.Session.SESSION_ID] = SessionIDCreate();
            ViewBag.SessionID = Session[Define.Session.SESSION_ID];
            return base.View(viewName, masterName, model);
        }
        private String SessionIDCreate()
        {
            DateTime pDate = DateTime.Now;
            Random ran = new Random(1000);
            return ran.Next(1000).ToString("0000") + pDate.ToString("yyyyMMddhhmmss");
        }
        protected ContentResult Empty()
        {
            return Content("");
        }
        protected bool SessionCheck(String name)
        {
            if (Session[name] != null && (bool)Session[name])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected ContentResult NoAjax()
        {
            return Content(null);
        }

        protected LanguageType? GetLanguageType()
        {
            String pLanguage = Request.Form[Define.Session.FORM_LANG];
            if (!String.IsNullOrEmpty(pLanguage))
            {
                if (String.Equals(Define.Session.FORM_KOREA, pLanguage))
                {
                    return LanguageType.Korea;
                }
                return LanguageType.Japan;
            }
            return null;
        }
    }
}