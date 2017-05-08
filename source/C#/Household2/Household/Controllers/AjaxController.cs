using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using System.Web.Security;
using Household.Filters;
using log4net;

namespace Household.Controllers
{
    [Household.Filters.ActionFilter]
    public partial class AjaxController : Controller
    {
        public ActionResult Apply(ApplyBean bean)
        {
            return new ApplyFlow(Request, Response, HttpContext, bean).Execute();
        }
        public ActionResult Modify(ApplyBean bean)
        {
            return new ModifyFlow(Request, Response, HttpContext, bean).Execute();
        }
        public ActionResult Delete(ApplyBean bean)
        {
            return new DeleteFlow(Request, Response, HttpContext, bean).Execute();
        }
        public ActionResult Search(SearchBean bean)
        {
            return new SearchFlow(Request, Response, HttpContext, bean).Execute(); 
        }
    }
}
