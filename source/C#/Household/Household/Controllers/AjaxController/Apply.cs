using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using Household.Dao;
using Household.Models.Master;
using Household.Models.Entity;
using System.Web.Security;
using Household.Filters;
using log4net;

namespace Household.Controllers
{
    public partial class AjaxController : AbstractController
    {
        [HttpPost]
        public ActionResult Apply(ApplyBean model)
        {
            return WorkAjax(model, (item, ret) =>
            {
                ApplyBean bean = item as ApplyBean;
                if (UserSession == null)
                {
                    ret.Result = Define.LOGIN_ERROR;
                    return ret;
                }
                if (!Validate(bean, true))
                {
                    ret.Result = Define.RESULT_NG;
                    ret.Error = Message.DATA_EROR;
                    return ret;
                }

                String date = String.Format("{0}-{1}-{2}", bean.HouseholdYear, bean.HouseholdMonth, bean.HouseholdDay);
                FactoryDao.Instance().GetHshldDao().InsertToInfo(UserSession.Grpd,
                                                                    UserSession.Usrd,
                                                                    bean.HouseholdCategory,
                                                                    bean.HouseholdType,
                                                                    date,
                                                                    bean.HouseholdContent,
                                                                    bean.Householdprice);
                ret.Result = Define.RESULT_OK;
                return ret;
            });
        }
    }
}