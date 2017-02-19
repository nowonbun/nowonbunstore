using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Household.Common;

namespace Household.Filters
{
    public class AuthorizeFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session[Define.USER_SESSION_NAME] == null)
            {
                return false;
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}