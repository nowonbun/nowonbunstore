using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using Household.Dao;
using Household.Models.Entity;
using System.Web.Security;
using Household.Filters;
using log4net;

namespace Household.Controllers
{
    [Household.Filters.ActionFilter]
    public partial class HomeController : AbstractController
    {
        private bool GetInfoByCookie(ref LoginBean bean, out DateTime connectDate)
        {
            bean.InputGroup = GetCookie("inputgroupid");
            bean.InuptId = GetCookie("inpuid");
            bean.InputPassword = GetCookie("inputpw");

            String year = GetCookie("date_year");
            String month = GetCookie("date_month");
            String day = GetCookie("date_day");
            if (!String.IsNullOrEmpty(year) && !String.IsNullOrEmpty(month) && !String.IsNullOrEmpty(day))
            {
                try
                {
                    connectDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                    return true;
                }
                catch { }
            }
            connectDate = new DateTime();
            return false;
        }
        private void Login(LoginBean bean, bool md5Crypt)
        {
            UsrNfDao dao = FactoryDao.Instance().GetUsrNfDao();
            UsrNf user = dao.SelectForSign(bean.InputGroup, bean.InuptId, md5Crypt ? Util.MD5HashCrypt(bean.InputPassword) : bean.InputPassword);
            if (user == null)
            {
                bean.ErrorMessage = Message.LOGIN_ERROR;
            }
            else
            {
                UserSession = user;
                FormsAuthentication.SetAuthCookie(user.Nm, false);
                if (!String.IsNullOrEmpty(bean.Remember))
                {
                    SetInfoToCookie();
                }
            }
        }

        private void ClearInfoToCookie()
        {
            SetCookie("inputgroupid", null);
            SetCookie("inpuid", null);
            SetCookie("inputpw", null);
            SetCookie("date_year", null);
            SetCookie("date_month", null);
            SetCookie("date_day", null);
        }
        private void SetInfoToCookie()
        {
            SetCookie("inputgroupid", UserSession.Grpd);
            SetCookie("inpuid", UserSession.Usrd);
            SetCookie("inputpw", UserSession.Pswrd);
            SetCookie("date_year", DateTime.Now.Year.ToString());
            SetCookie("date_month", DateTime.Now.Month.ToString());
            SetCookie("date_day", DateTime.Now.Day.ToString());
        }
    }
}
