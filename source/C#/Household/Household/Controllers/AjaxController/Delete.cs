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
        public ActionResult Delete(ApplyBean model)
        {
            return WorkAjax(model, (item, ret) =>
            {
                ApplyBean bean = item as ApplyBean;
                if (UserSession == null)
                {
                    ret.Result = Define.LOGIN_ERROR;
                    return ret;
                }
                if (String.IsNullOrEmpty(bean.HouseholdIdx) || String.IsNullOrEmpty(bean.HouseholdPdt))
                {
                    ret.Result = Define.RESULT_NG;
                    ret.Error = Message.DATA_EROR;
                    return ret;
                }
                Hshld hshld = FactoryDao.Instance().GetHshldDao().SelectByIdx(UserSession.Grpd, bean.HouseholdIdx);
                if (hshld == null || !String.Equals(hshld.Pdt.ToString(Define.PDT_FORMAT), bean.HouseholdPdt))
                {
                    ret.Result = Define.RESULT_NG;
                    ret.Error = Message.DATA_CHECK;
                    return ret;
                }
                if (!String.Equals(hshld.Grpd, UserSession.Grpd))
                {
                    ret.Result = Define.LOGIN_ERROR;
                    ret.Error = Message.DATA_EROR;
                    return ret;
                }

                FactoryDao.Instance().GetHshldLogDao().InsertToLog(hshld);
                FactoryDao.Instance().GetHshldDao().DeleteToInfo(hshld.Ndx.ToString());
                ret.Result = Define.RESULT_OK;
                return ret;
            });
        }
    }
}