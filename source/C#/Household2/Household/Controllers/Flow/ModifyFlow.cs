using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;

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
            return true;
        }
        public override ActionResult Run()
        {
            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }
        public override ActionResult Error()
        {
            return null;
        }
    }
}