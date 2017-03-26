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
    public class DeleteFlow: AbstractFlow
    {
        [ResourceDao]
        private IHshldDao hshldDao;

        [ResourceDao]
        private IHshldLogDao hshldLogDao;

        private ApplyBean model;

        public DeleteFlow(HttpRequestBase request, HttpResponseBase response, HttpContextBase context, ApplyBean model)
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
            if (String.IsNullOrEmpty(model.HouseholdIdx) || String.IsNullOrEmpty(model.HouseholdPdt))
            {
                ResultBean.Result = Define.RESULT_NG;
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
            hshldDao.DeleteToInfo(hshld.Ndx.ToString());
            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }
    }
}