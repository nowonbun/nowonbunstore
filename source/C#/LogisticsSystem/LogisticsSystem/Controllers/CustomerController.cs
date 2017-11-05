using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using LogisticsSystem.Dao;
using LogisticsSystem.Master;
using LogisticsSystem.Validate;

namespace LogisticsSystem.Controllers
{
    public class CustomerController : AbstractController
    {
        private const int PAGELIMIT = 10;
        private CustomerInfoDao customerInfoDao = FactoryDao.Instance().GetCustomerInfoDao();
        /// <summary>
        /// 고객 등록 페이지 초기화
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult Main()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/CommentApply - Error");
                return base.Logout();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/Main");
            LanguageType? lType = GetLanguageType();
            ViewBag.customerType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.CUSTOMER_TYPE, lType);
            ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
            ViewBag.customerTaxType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.CUSTOMER_TAX_TYPE, lType);

            //1페이지 검색
            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)customerInfoDao.GetCustomerInfoCount(CompanySession.CompanyCode) / (Double)PAGELIMIT)));
            ViewBag.list = customerInfoDao.SelectList(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode);
            ViewBag.listcount = count;

            return View("~/Views/Customer/Web/Main.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 검색Ajax
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ListSearch(int page = 0)/*Ajax*/
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/CommentApply - Error");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/ListSearch");
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = customerInfoDao.GetCustomerInfoCount(CompanySession.CompanyCode);
            IList<CustomerInfo> list = customerInfoDao.SelectList(PAGELIMIT, page, CompanySession.CompanyCode);

            Dictionary<String, object> pRet = new Dictionary<String, object>();
            for (int i = 0; i < list.Count; i++)
            {
                pRet.Add("item" + i.ToString(), list[i]);
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", PAGELIMIT);

            return Json(pRet, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 코드생성
        /// </summary>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult CodeCreate()
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/CodeCreate");
            return Content(customerInfoDao.CreateCode());
        }
        /// <summary>
        /// 데이터 입력
        /// </summary>      
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Insert(CustomerInfo data)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return Content("SESSIONOUT");
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/Insert");

            List<String> Errmsg = data.Validate((LanguageType?)Session["languageType"]);
            if (Errmsg.Count > 1)
            {
                String ret = "";
                foreach (string pBuffer in Errmsg)
                {
                    ret += pBuffer + "<br>";
                }
                return Content(ret);
            }

            data.CustomerCode = "CC-" + data.CustomerCode;
            data.Creater = User.Identity.Name;
            data.CreateDate = DateTime.Now;
            data.State = "0";
            data.ConvertPostNumber();
            data.CompanyCode = CompanySession.CompanyCode;
            customerInfoDao.InsertCustmer(data);
            return Content(null);
        }
        /// <summary>
        /// 리스트클릭시 검색
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult CustomerSearch(String code)/*Ajax*/
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return NoAjax();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/CustomerSearch");
            CustomerInfo info = customerInfoDao.SelectCustomer(code, CompanySession.CompanyCode);
            return Json(info, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 히스토리 리스트 검색
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult HistorySearch(String code, int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/HistorySearch");
            int pagelimit = 10;
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = customerInfoDao.GetCustomerInfoHistoryCount(code, CompanySession.CompanyCode);
            IList<CustomerInfo> list = customerInfoDao.SearchCustomerInfoHistory(pagelimit, page, code, CompanySession.CompanyCode);

            Dictionary<String, object> pRet = new Dictionary<String, object>();
            for (int i = 0; i < list.Count; i++)
            {
                pRet.Add("item" + i.ToString(), list[i]);
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", pagelimit);

            return Json(pRet, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 히스토리 리스트에서 선택시 검색
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult CustomerHistorySearch(int idx = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/CustomerHistorySearch");
            CustomerInfo info = customerInfoDao.SelectCustomerHistory(idx, CompanySession.CompanyCode);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 삭제쿼리
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Delete(String code)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return Content("SESSIONOUT");
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/CustomerHistorySearch");
            customerInfoDao.DeleteCustomer(code, CompanySession.CompanyCode);
            return Content("OK");

        }
        /// <summary>
        /// 수정처리
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Modify(CustomerInfo data)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return Content("SESSIONOUT");
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Customer/Modify");
            List<String> Errmsg = data.Validate((LanguageType?)Session["languageType"]);
            if (Errmsg.Count > 0)
            {
                String ret = "";
                foreach (string pBuffer in Errmsg)
                {
                    ret += pBuffer + "<br>";
                }
                return Content(ret);
            }
            Delete("CC-" + data.CustomerCode);
            return Insert(data);
        }
    }
}

