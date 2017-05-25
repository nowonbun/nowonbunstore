using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using System;

namespace Household.Controllers
{
    [Household.Filters.ActionFilter]
    public partial class AjaxController : AbstractController
    {
        public ActionResult Apply(ApplyBean bean)
        {
            try
            {
                return new ApplyFlow(Request, Response, HttpContext, bean).Execute();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Error();
            }
        }
        public ActionResult Modify(ApplyBean bean)
        {
            try
            {
                return new ModifyFlow(Request, Response, HttpContext, bean).Execute();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Error();
            }
        }
        public ActionResult Delete(ApplyBean bean)
        {
            try
            {
                return new DeleteFlow(Request, Response, HttpContext, bean).Execute();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Error();
            }
        }
        public ActionResult Search(SearchBean bean)
        {
            try
            {
                return new SearchFlow(Request, Response, HttpContext, bean).Execute();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Error();
            }
        }

        private ActionResult Error()
        {
            AjaxResultBean ResultBean = new AjaxResultBean();
            ResultBean.Result = Define.RESULT_NG;
            ResultBean.Error = Message.EXCEPTION;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }
    }
}
