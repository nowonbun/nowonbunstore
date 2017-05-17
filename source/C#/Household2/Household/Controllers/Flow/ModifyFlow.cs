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
    public class ModifyFlow : AbstractFlow
    {
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
            String json = HttpConnector.GetInstance().GetDataRequest("GetHousehold", new Dictionary<String, Object>()
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
            String date = String.Format("{0}-{1}-{2}", model.HouseholdYear, model.HouseholdMonth, model.HouseholdDay);
            HttpConnector.GetInstance().GetDataRequest("ModifyHousehold", new Dictionary<String, Object>()
            {
                {"IDX",model.HouseholdIdx},
                {"GID",UserSession.Id},
                {"CD",model.HouseholdCategory},
                {"TP",model.HouseholdType},
                {"DT",date},
                {"CNTXT",model.HouseholdContent},
                {"PRC",model.Householdprice}
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