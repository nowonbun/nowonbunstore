using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;

namespace Household.Controllers
{
    public class ApplyFlow : AbstractFlow
    {
        private ApplyBean model;

        public ApplyFlow(HttpRequestBase request, HttpResponseBase response, HttpContextBase context, ApplyBean model)
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
            return true;
        }

        public override ActionResult Run()
        {
            String date = String.Format("{0}-{1}-{2}", model.HouseholdYear, model.HouseholdMonth, model.HouseholdDay);

            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }
    }
}