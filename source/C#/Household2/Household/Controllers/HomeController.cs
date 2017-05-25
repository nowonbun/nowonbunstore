using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using System.Web.Security;
using Household.Filters;
using Newtonsoft.Json;

namespace Household.Controllers
{
    [Household.Filters.ActionFilter]
    public partial class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            base.Logger.Info("Index Open");
            return View("~/Views/login.cshtml", "~/Views/master.cshtml");
        }

        public new ActionResult Redirect(String code)
        {
            try
            {
                base.Logger.Info("Redirect Open");
                string data = HttpConnector.GetInstance().GetRequest("https://accounts.google.com/o/oauth2/token",
                                                        HttpConnector.HttpMethod.POST,
                                                        new Dictionary<String, Object>() {
                                                    { "code", code },
                                                    { "client_id", HtmlUtil.GetClientID()},
                                                    { "client_secret", HtmlUtil.GetClientSecret()},
                                                    { "redirect_uri", HtmlUtil.GetRedirectUrl()},
                                                    { "grant_type", "authorization_code"}});
                base.Logger.Info("Google account auth : " + code);
                LoginToken token = JsonConvert.DeserializeObject<LoginToken>(data);
                data = HttpConnector.GetInstance().GetRequest("https://www.googleapis.com/oauth2/v1/userinfo",
                                                 HttpConnector.HttpMethod.GET,
                                                 new Dictionary<String, Object>() {
                                             { "access_token", token.Access_token }});
                base.Logger.Info("Google access_token : " + token.Access_token);
                LoginBean login = JsonConvert.DeserializeObject<LoginBean>(data);
                login.Token = token;

                String usercheck = HttpConnector.GetInstance().GetDataRequest("CheckUser.php",
                                                                new Dictionary<String, Object>() {
                                                            { "GID", login.Id } });
                Session["USER_BUFFER"] = login;
                if (usercheck == null)
                {
                    base.Logger.Info("usercheck == null");
                    return base.Redirect("/Home/ApplyConfirm");
                }
                base.Logger.Info("Redirect -> Apply");
                return Apply();
            }
            catch (Exception e)
            {
                base.Logger.Error(e);
                return base.Redirect("/");
            }
        }

        public ActionResult ApplyConfirm()
        {
            base.Logger.Info("ApplyConfirm Open");
            return View("~/Views/applycorfirm.cshtml", "~/Views/master.cshtml");
        }

        public ActionResult Apply()
        {
            try
            {
                base.Logger.Info("Apply Open");
                LoginBean login = Session["USER_BUFFER"] as LoginBean;
                Session["USER_BUFFER"] = null;
                HttpConnector.GetInstance().GetDataRequest("ApplyUser.php",
                                             new Dictionary<String, Object>() {
                                         { "GID", login.Id },
                                         {"NAME",login.Name},
                                         {"EMAIL",""}});
                UserSession = login;
                FormsAuthentication.SetAuthCookie(login.Id, false);
                return base.Redirect("/Home/Main");
            }
            catch (Exception e)
            {
                base.Logger.Error(e);
                return base.Redirect("/");
            }
        }

        [AuthorizeFilter]
        public ActionResult Main()
        {
            base.Logger.Info("Main Open");
            return View("~/Views/main.cshtml", "~/Views/master.cshtml");
        }

        public ActionResult Signout()
        {
            base.Logger.Info("Signout Open");
            FormsAuthentication.SignOut();
            Session.Clear();
            Request.Cookies.Clear();
            return base.Redirect("/");
        }
    }
}
