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

        public new ActionResult Redirect(String code)
        {
            try
            {
                string data = HttpConnector.GetInstance().GetRequest("https://accounts.google.com/o/oauth2/token",
                                                        HttpConnector.HttpMethod.POST,
                                                        new Dictionary<String, String>() { 
                                                    { "code", code },
                                                    { "client_id", HtmlUtil.GetClientID()},
                                                    { "client_secret", HtmlUtil.GetClientSecret()},
                                                    { "redirect_uri", HtmlUtil.GetRedirectUrl()},
                                                    { "grant_type", "authorization_code"}});
                LoginToken token = JsonConvert.DeserializeObject<LoginToken>(data);
                data = HttpConnector.GetInstance().GetRequest("https://www.googleapis.com/oauth2/v1/userinfo",
                                                 HttpConnector.HttpMethod.GET,
                                                 new Dictionary<String, String>() { 
                                             { "access_token", token.Access_token }});

                LoginBean login = JsonConvert.DeserializeObject<LoginBean>(data);
                login.Token = token;

                String usercheck = HttpConnector.GetInstance().GetDataRequest("CheckUser",
                                                                new Dictionary<String, String>() { 
                                                            { "GID", login.Id } });
                Session["USER_BUFFER"] = login;
                if ("FALSE".Equals(usercheck.ToUpper()))
                {
                    return base.Redirect("/Home/ApplyConfirm");
                }
                return Apply();
            }
            catch (Exception)
            {
                return base.Redirect("/");
            }
        }

        public ActionResult ApplyConfirm()
        {
            return View("~/Views/applycorfirm.cshtml", "~/Views/master.cshtml");
        }

        public ActionResult Apply()
        {
            LoginBean login = Session["USER_BUFFER"] as LoginBean;
            Session["USER_BUFFER"] = null;
            HttpConnector.GetInstance().GetDataRequest("ApplyUser",
                                         new Dictionary<String, String>() { 
                                         { "GID", login.Id },
                                         {"NAME",login.Name},
                                         {"EMAIL",""}});
            UserSession = login;
            FormsAuthentication.SetAuthCookie(login.Id, false);
            return base.Redirect("/Home/Main");
        }

        [AuthorizeFilter]
        public ActionResult Main()
        {
            return View("~/Views/main.cshtml", "~/Views/master.cshtml");
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Request.Cookies.Clear();
            return base.Redirect("/");
        }
    }
}
