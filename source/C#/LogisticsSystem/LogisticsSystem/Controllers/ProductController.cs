using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LogisticsSystem.Models;
using LogisticsSystem.App_Code;
using LogisticsSystem.Dao;
using LogisticsSystem.Master;
using LogisticsSystem.Validate;

namespace LogisticsSystem.Controllers
{
    public class ProductController : AbstractController
    {
        private const int PAGELIMIT = 10;

        private ProductInfoDao productInfoDao = FactoryDao.Instance().GetProductInfoDao();

        /// <summary>
        /// 상품 등록 페이지 초기화
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult Main()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Main - Error");
                return Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Main");
            LanguageType? lType = GetLanguageType();

            ViewBag.productType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.PRODUCT_TYPE, lType);
            ViewBag.productSpec = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.PRODUCT_SPEC, lType);
            ViewBag.list = productInfoDao.SelectList(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode);
            ViewBag.listcount = Convert.ToInt32(Math.Ceiling((Double)((Double)productInfoDao.SelectProductInfoCount(CompanySession.CompanyCode) / (Double)PAGELIMIT)));

            return View("~/Views/Product/Web/Main.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 검색 AJAX
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ListSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/ListSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/ListSearch");
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = productInfoDao.SelectProductInfoCount(CompanySession.CompanyCode);
            IList<ProductInfo> list = productInfoDao.SelectList(PAGELIMIT, page, CompanySession.CompanyCode);
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/CodeCreate - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/CodeCreate");
            return Content(productInfoDao.CreateCode());
        }
        /// <summary>
        /// 상품 등록부분 AJAX
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Insert(ProductInfo data)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Insert - Sessionout");
                return Content("SESSIONOUT");
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Insert");

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
            data.ProductCode = "GC-" + data.ProductCode;
            data.Creater = User.Identity.Name;
            data.CreateDate = DateTime.Now;
            data.State = "0";
            data.CompanyCode = CompanySession.CompanyCode;
            productInfoDao.InsertProduct(data);
            return Content(null);
        }
        /// <summary>
        /// 상품검색(리스트 클릭시)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ProductSearch(String code)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/ProductSearch - Sessionout");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/ProductSearch");
            ProductInfo info = productInfoDao.SelectProduct(code, CompanySession.CompanyCode);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 삭제함수
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Delete - Sessionout");
                return Content("SESSIONOUT");
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Delete");
            productInfoDao.DeleteProduct(code, CompanySession.CompanyCode);
            return Content("OK");
        }
        /// <summary>
        /// 수정합수
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Modify(ProductInfo data)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Modify - Sessionout");
                return Content("SESSIONOUT");
            }
            //버그가 있다.. 수정해야겠다.
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/Modify");
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
            Delete("GC-" + data.ProductCode);
            return Insert(data);
        }
        /// <summary>
        /// 상품 이력 리스트 검색 Ajax
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
            int pagelimit = 10;
            if (page < 1)
            {
                page = 1;
            }
            int count = productInfoDao.GetProductInfoHistoryCount(code, CompanySession.CompanyCode);
            IList<ProductInfo> list = productInfoDao.SearchProductInfoHistory(pagelimit, page, code, CompanySession.CompanyCode);
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
        /// 히스토리 리스트에서 상품검색을 할 경우
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ProductHistorySearch(int idx = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                return NoAjax();
            }
            ProductInfo info = productInfoDao.SelectProductHistory(idx, CompanySession.CompanyCode);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}
