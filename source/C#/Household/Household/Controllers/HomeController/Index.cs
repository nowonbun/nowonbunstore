using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using HouseholdORM;
using System.Web.Security;
using Household.Filters;
using log4net;

namespace Household.Controllers
{
    public partial class HomeController : AbstractController
    {
        [ResourceDao]
        protected ICnctLgDao cnctLgDao;

        public ActionResult Index(LoginBean bean)
        {
            if (String.Equals(bean.Post, Define.POST_PAGE))
            {
                if (String.IsNullOrEmpty(bean.InputGroup))
                {
                    bean.ErrorMessage = Message.LOGIN_ERROR_NOT_GROUPID;
                }
                else if (String.IsNullOrEmpty(bean.InuptId))
                {
                    bean.ErrorMessage = Message.LOGIN_ERROR_NOT_ID;
                }
                else if (String.IsNullOrEmpty(bean.InputPassword))
                {
                    bean.ErrorMessage = Message.LOGIN_ERROR_NOT_PWD;
                }
                else
                {
                    Login(bean, true);
                }
            }
            else
            {
                DateTime time;
                if (GetInfoByCookie(ref bean, out time))
                {
                    bean.Remember = "1";
                    if (DateTime.Compare(DateTime.Now.AddDays(-7), time) <= 0)
                    {
                        Login(bean, false);
                    }
                }
            }
            if (UserSession != null)
            {
                cnctLgDao.InsertToSignin(UserSession.Grpd, UserSession.Usrd);
                return Redirect("/Home/Main");
            }
            ViewBag.ModelView = bean;
            return View("~/Views/login.cshtml", "~/Views/master.cshtml");
        }
    }
}