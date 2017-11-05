using System;
using System.Web;
using System.Web.Mvc;

namespace LogisticsSystem.App_Code
{
    public class PipelineFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            session[Define.Session.CONTROLLER] = filterContext.RouteData.Values[Define.Session.CONTROLLER];
            session[Define.Session.ACTION] = filterContext.RouteData.Values[Define.Session.ACTION];
            session[Define.Session.SESSION_CHECK] = true;
            session[Define.Session.BROWSER_CHECK] = true;

            //String master = (String)session[Define.Session.MASTER];
            //if (Object.Equals(Define.MASTER_VIEW,master))
            //{
            //    String SessionID = (String)session[Define.Session.SESSION_ID];
            //    if (SessionID != null)
            //    {
            //        if (false && !SessionID.Equals(filterContext.HttpContext.Request.Form["SessionID"]))
            //        {
            //            filterContext.HttpContext.Session.Clear();
            //            filterContext.HttpContext.Session.RemoveAll();
            //            session["SessionCheck"] = false;
            //        }
            //    }
            //}
            //if (filterContext.HttpContext.Request.Browser.Browser.ToLower().IndexOf("explorer") != -1 ||
            //    filterContext.HttpContext.Request.Browser.Browser.ToLower().IndexOf("ie") != -1
            //    )
            //{
            //    if (false && Convert.ToDouble(filterContext.HttpContext.Request.Browser.Version) < 10)
            //    {
            //        filterContext.HttpContext.Session.Clear();
            //        filterContext.HttpContext.Session.RemoveAll();
            //        session["BrowserCheck"] = false;
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}