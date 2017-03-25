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
    public partial class HomeController : AbstractController
    {
        [AuthorizeFilter]
        public ActionResult Main()
        {
            return View("~/Views/main.cshtml", "~/Views/master.cshtml");
        }
    }
}