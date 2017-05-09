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
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Household.Controllers
{
    [Household.Filters.ActionFilter]
    public partial class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            return View("~/Views/login.cshtml", "~/Views/master.cshtml");
        }

        public ActionResult Redirect(String code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("code=").Append(code).Append("&");
            sb.Append("client_id=").Append(HtmlUtil.GetClientID()).Append("&");
            sb.Append("client_secret=").Append(HtmlUtil.GetClientSecret()).Append("&");
            sb.Append("redirect_uri=").Append(HtmlUtil.GetRedirectUrl()).Append("&");
            sb.Append("grant_type=").Append("authorization_code");
            string data = Util.GetWebPostRequest("https://accounts.google.com/o/oauth2/token", sb.ToString());
            LoginToken token = JsonConvert.DeserializeObject<LoginToken>(data);

            data = Util.GetWebGetRequest("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + token.Access_token);
            LoginBean login = JsonConvert.DeserializeObject<LoginBean>(data);
            login.Token = token;

            /**
             * TODO: data-center-process
             **/
            return base.Redirect("/Home/ApplyConfirm");
        }

        public ActionResult ApplyConfirm()
        {
            return View("~/Views/applycorfirm.cshtml", "~/Views/master.cshtml");
        }

        public ActionResult Apply()
        {
            return Redirect("/Home/Main");
        }

        public ActionResult Main()
        {
            return View("~/Views/main.cshtml", "~/Views/master.cshtml");
        }
    }
}
