using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;
using Household.Models.Master;

namespace Household.Controllers
{
    public class ModifyFlow : AbstractFlow
    {
        [ResourceDao]
        private IHshldDao hshldDao;

        [ResourceDao]
        private IHshldLogDao hshldLogDao;

        private ApplyBean model;

        public ModifyFlow(HttpRequestBase request, HttpResponseBase response, HttpContextBase context, ApplyBean model)
            : base(request, response, context)
        {
            this.model = model;
        }
        public override bool Validate()
        {
            if (UserSession == null)
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdIdx))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdYear))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdMonth))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdDay))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdCategory))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdType))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.HouseholdContent))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.Householdprice))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            Hshld hshld = hshldDao.SelectByIdx(UserSession.Grpd, model.HouseholdIdx);
            if (hshld == null || !String.Equals(hshld.Pdt.ToString(Define.PDT_FORMAT), model.HouseholdPdt))
            {
                ResultBean.Result = Define.RESULT_NG;
                ResultBean.Error = Message.DATA_CHECK;
                return false;
            }
            if (!String.Equals(hshld.Grpd, UserSession.Grpd))
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            return true;
        }
        public override ActionResult Run()
        {
            Hshld hshld = hshldDao.SelectByIdx(UserSession.Grpd, model.HouseholdIdx);

            hshldLogDao.InsertToLog(hshld);
            String date = String.Format("{0}-{1}-{2}", model.HouseholdYear, model.HouseholdMonth, model.HouseholdDay);
            hshldDao.UpdateToInfo(hshld.Ndx.ToString(),
                                    model.HouseholdCategory,
                                    model.HouseholdType,
                                    date,
                                    model.HouseholdContent,
                                    model.Householdprice);
            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }
        public override ActionResult Error()
        {
            return null;
        }
    }
}