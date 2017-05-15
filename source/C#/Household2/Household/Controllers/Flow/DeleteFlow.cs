using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;
using Household.Models.Entity;
using Household.Models.Master;
using Newtonsoft.Json;


namespace Household.Controllers
{
    public class DeleteFlow: AbstractFlow
    {
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
            String json = HttpConnector.GetInstance().GetDataRequest("GetHousehold", new Dictionary<String, String>()
            {
                {"GID",UserSession.Id},
                {"IDX",model.HouseholdIdx}
            });
            HouseHold household = JsonConvert.DeserializeObject<HouseHold>(json);
            if (household == null || !String.Equals(household.Createdate.ToString(Define.PDT_FORMAT), model.HouseholdPdt))
            {
                ResultBean.Result = Define.RESULT_NG;
                ResultBean.Error = Message.DATA_CHECK;
                return false;
            }
            return true;
        }

        public override ActionResult Run()
        {
            HttpConnector.GetInstance().GetDataRequest("DeleteHousehold", new Dictionary<String, String>()
            {
                {"IDX",model.HouseholdIdx},
                {"GID",UserSession.Id}
            });
            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }
    }
}