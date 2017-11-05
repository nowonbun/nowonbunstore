using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LogisticsSystem.App_Code
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase context = filterContext.HttpContext;
            if (context.User.Identity.IsAuthenticated &&
                context.Session[Define.Session.USER_INFO] != null &&
                context.Session[Define.Session.COMPANY_INFO] != null)
            {
                FormsAuthentication.SetAuthCookie(filterContext.HttpContext.User.Identity.Name, false);
                context.Session[Define.Session.AUTH_CHECK] = true;
            }
            else
            {
                FormsAuthentication.SignOut();
                context.Session.Clear();
                context.Session.RemoveAll();
                context.Session[Define.Session.AUTH_CHECK] = false;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}