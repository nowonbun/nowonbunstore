using System;
using System.Web;
using System.Web.Mvc;

namespace LogisticsSystem.App_Code
{
    public class AjaxFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (filterContext.HttpContext.Request.Cookies[Define.Session.SESSION_ID] == null)
            {
                session[Define.Session.AJAX_CHECK] = false;
            }
            else
            {
                String cookiesSessionID = filterContext.HttpContext.Request.Cookies[Define.Session.SESSION_ID].Value;
                String sessionID = session[Define.Session.SESSION_ID].ToString();
                if (sessionID != null && sessionID.Equals(cookiesSessionID))
                {
                    session[Define.Session.AJAX_CHECK] = true;
                }
                else
                {
                    session[Define.Session.AJAX_CHECK] = false;
                }
            }
        }
    }
}