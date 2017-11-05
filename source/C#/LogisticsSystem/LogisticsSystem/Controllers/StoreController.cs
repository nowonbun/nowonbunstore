using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using LogisticsSystem.Dao;
using LogisticsSystem.Master;
using LogisticsSystem.Validate;

namespace LogisticsSystem.Controllers
{
    public class StoreController : AbstractController
    {
        private const int COMMIT_KANRYOU = 1;
        private const int COMMIT_KAKUNINN = 2;
        private const int PAGELIMIT = 50;

        private const int COMMIT_SHOUNINNPAGE = 1;
        private const int COMMIT_SHOUNINN = 2;
        private const int COMMIT_SHOUNINNCANCEL = 3;

        private const String DOCUMENTSTORECODE = "1";

        private ProductInfoDao productInfoDao = FactoryDao.Instance().GetProductInfoDao();
        private ProductFlowDao productFlowDao = FactoryDao.Instance().GetProductFlowDao();
        private CustomerInfoDao customerInfoDao = FactoryDao.Instance().GetCustomerInfoDao();
        private DocumentDao documentDao = FactoryDao.Instance().GetDocumentDao();
        private OrderTableDao orderTableDao = FactoryDao.Instance().GetOrderTableDao();
        private OrderTableSubDao orderTableSubDao = FactoryDao.Instance().GetOrderTableSubDao();
        private CargoDao cargoDao = FactoryDao.Instance().GetCargoDao();

        /// <summary>
        /// 입고등록
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ApplyAdd(String key)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyAdd - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyAdd");

            if (String.Equals("BACK", key))
            {
                ProductFlow pFlow = (ProductFlow)Session["productFlow"];
                if (pFlow != null)
                {
                    ViewBag.Flow = pFlow;
                    ViewBag.totalprice = pFlow.ProductBuyPrice * pFlow.ProductAmount;
                }
            }

            ViewBag.productlist = productInfoDao.GetProductNameList(CompanySession.CompanyCode); ;
            ViewBag.userInfo = UserSession;
            //세션 초기화
            Session["productFlow"] = null;
            return View("~/Views/Store/Web/ApplyAdd.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 상품검색(등록화면에서) AJAX
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ProductSelect(int idx)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ProductSelect - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ProductSelect");
            LanguageType? lType = GetLanguageType();

            ProductInfo info = productInfoDao.SelectProduct(idx, CompanySession.CompanyCode, lType);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 입고등록입력확인
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApplyAddCheck(ProductFlow pFlow)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyAddCheck - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyAddCheck");
            LanguageType? lType = GetLanguageType();
            Session[Define.Session.ACTION] = "ApplyAdd";
            List<String> pErrmsg = pFlow.Validate(lType);
            IList<ProductInfo> productList = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            //에러가 있는 경우
            if (pErrmsg.Count > 0)
            {
                String err = "";
                foreach (String pData in pErrmsg)
                {
                    err += pData + "<br>";
                }
                ViewBag.ErrMsg = err;
                ViewBag.Flow = pFlow;
                ViewBag.totalprice = pFlow.ProductBuyPrice * pFlow.ProductAmount;
                ViewBag.productlist = productList;
                ViewBag.userInfo = UserSession;

                return View("~/Views/Store/Web/ApplyAdd.cshtml", Define.MASTER_VIEW);
            }
            foreach (ProductInfo pData in productList)
            {
                if (Object.Equals(pData.Idx, pFlow.ProductIndex))
                {
                    pFlow.ProductName = pData.ProductName;
                    break;
                }
            }
            ViewBag.Flow = pFlow;
            ViewBag.totalprice = pFlow.ProductBuyPrice * pFlow.ProductAmount;
            Session["productFlow"] = pFlow;

            ViewBag.Commit = COMMIT_KAKUNINN;
            return View("~/Views/Store/Web/ApplyAddCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 입고등록입력확인
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApplyInsert()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyInsert - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyInsert");

            ProductFlow flow = (ProductFlow)Session["productFlow"];
            flow.State = Define.ProductFlow.INCOMESTANBY.ToString();
            flow.ApplyType = Define.ProductFlow.APPLYTYPE_NORMAL;
            flow.CompanyCode = CompanySession.CompanyCode;
            String pBuffer = flow.ProductName;
            flow.ProductName = null;
            productFlowDao.InsertProductFlow(flow);
            flow.ProductName = pBuffer;
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductBuyPrice * flow.ProductAmount;

            //세션 초기화
            Session["productFlow"] = null;

            ViewBag.Commit = COMMIT_KANRYOU;
            Session["action"] = "ApplyAdd";
            return View("~/Views/Store/Web/ApplyAddCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 입고승인리스트
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ApplyCheckList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyCheckList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyCheckList");
            ViewBag.list = productFlowDao.SelectFlow(PAGELIMIT, 1, Define.ProductFlow.INCOMESTANBY, CompanySession.CompanyCode);
            ViewBag.listcount = Convert.ToInt32(Math.Ceiling((Double)((Double)productFlowDao.GetProductFlowCount(Define.ProductFlow.INCOMESTANBY, CompanySession.CompanyCode) / (Double)PAGELIMIT)));
            return View("~/Views/Store/Web/ApplyCheckList.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 승인리스트 검색(AJAX)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApplyListSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyListSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyListSearch");
            if (page < 1)
            {
                page = 1;
            }
            int count = productFlowDao.GetProductFlowCount(Define.ProductFlow.INCOMESTANBY, CompanySession.CompanyCode);
            IList<ProductFlow> list = productFlowDao.SelectFlow(PAGELIMIT, page, Define.ProductFlow.INCOMESTANBY, CompanySession.CompanyCode);
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
        /// 입고승인페이지
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApplyApprovePage(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyApprovePage - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyApprovePage");
            LanguageType? lType = GetLanguageType();
            Dictionary<String, Object> sessionBuffer = new Dictionary<string, object>();
            ProductFlow flow = productFlowDao.SelectProductFlow(idx, CompanySession.CompanyCode);
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductBuyPrice * flow.ProductAmount;
            sessionBuffer.Add("FLOW", flow);
            if (flow.ApplyType != 0)
            {
                //발주서 검색
                OrderTable order = orderTableDao.SelectOrderTable(flow.ApplyType, CompanySession.CompanyCode);

                //정상경로가 아님
                if (order.Idx <= 0)
                {
                    return ErrorPage("/Home/Error");
                }
                sessionBuffer.Add("ORDER", order);
                //문서정보 취득
                Document doc = documentDao.SelectDocument(order.Idx, DOCUMENTSTORECODE, CompanySession.CompanyCode);
                sessionBuffer.Add("DOCUMENT", doc);

                //서버 상품 검색
                IList<OrderTableSub> orderSubList = orderTableSubDao.SelectSubList(order.Idx, CompanySession.CompanyCode, lType);
                sessionBuffer.Add("SUBLIST", orderSubList);

                //기본 데이터 Attribute저장
                ViewBag.doc = doc;
                ViewBag.order = order;
                ViewBag.orderSub = orderSubList;

                //전체 금액 계산
                ViewBag.totalMoney = orderSubList.Sum((pSub) => { return pSub.ProductMoney; });
                //코드마스터 취득
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
            }

            Session["ApproveData"] = sessionBuffer;
            ViewBag.Commit = COMMIT_SHOUNINNPAGE;
            Session[Define.Session.ACTION] = "ApplyCheckList";
            return View("~/Views/Store/Web/ApplyCheckPage.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 입고승인
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApplyApprove(String key)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyApprove - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyApprove");

            Dictionary<String, Object> sessionBuffer = (Dictionary<String, Object>)Session["ApproveData"];
            if (sessionBuffer == null)
            {
                return ErrorPage("/Home/Error");
            }
            Session["ApproveData"] = null;
            LanguageType? lType = GetLanguageType();

            ProductFlow pFlow = (ProductFlow)sessionBuffer["FLOW"];
            ViewBag.Flow = pFlow;
            ViewBag.totalprice = pFlow.ProductBuyPrice * pFlow.ProductAmount;
            if (pFlow.ApplyType != 0)
            {
                OrderTable order = (OrderTable)sessionBuffer["ORDER"];
                Document doc = (Document)sessionBuffer["DOCUMENT"];
                IList<OrderTableSub> orderSubList = (IList<OrderTableSub>)sessionBuffer["SUBLIST"];
                //코드마스터 취득
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster("MoneySendType", lType);

                //기본 데이터 Attribute저장
                ViewBag.doc = doc;
                ViewBag.order = order;
                ViewBag.orderSub = orderSubList;

                //전체 금액 계산
                ViewBag.totalMoney = orderSubList.Sum((pSub) => { return pSub.ProductMoney; });
            }

            //등록부분
            if (String.Equals(Define.APPROVE, key))
            {
                Cargo cargo = new Cargo();
                productFlowDao.ApproveProduct(pFlow.Idx, CompanySession.CompanyCode, Define.ProductFlow.INCOMECOMPLATE);
                cargo.ProductIndex = pFlow.ProductIndex;
                cargo.ProductInput = pFlow.ProductAmount;
                cargo.ProductOutput = 0;
                cargo.ProductMoney = pFlow.ProductAmount * pFlow.ProductBuyPrice;
                cargo.Creater = UserSession.Creater;
                cargo.CreateDate = DateTime.Now;
                cargo.State = Define.STATE_NORMAL.ToString();
                cargo.CompanyCode = CompanySession.CompanyCode;
                cargoDao.InsertCargo(cargo);
                ViewBag.Commit = COMMIT_SHOUNINN;
            }
            else
            {
                productFlowDao.ApproveProduct(pFlow.Idx, CompanySession.CompanyCode, Define.ProductFlow.INCOMECANCEL);
                ViewBag.Commit = COMMIT_SHOUNINNCANCEL;
            }
            Session[Define.Session.ACTION] = "ApplyCheckList";
            return View("~/Views/Store/Web/ApplyCheckPage.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 입고리스트(히스토리)
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ApplyList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyListearch");
            LanguageType? lType = GetLanguageType();

            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)productFlowDao.CountFlowList(CompanySession.CompanyCode, Define.ProductFlow.INCOMECANCEL, Define.ProductFlow.INCOMECOMPLATE, Define.ProductFlow.INCOMESTANBY) / (Double)PAGELIMIT)));
            IList<ProductFlow> list = productFlowDao.SelectFlowList(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode, Define.ProductFlow.INCOMECANCEL, Define.ProductFlow.INCOMECOMPLATE, Define.ProductFlow.INCOMESTANBY);
            list.AsParallel().ForAll((pFlow) => { pFlow.stateView(lType); });
            ViewBag.list = list;
            ViewBag.listcount = count;

            return View("~/Views/Store/Web/ApplyList.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 리스트검색Ajax
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApplyListearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyListearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApplyListearch");
            LanguageType? lType = GetLanguageType();
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = productFlowDao.CountFlowList(CompanySession.CompanyCode, 1, 2, 5);
            IList<ProductFlow> list = productFlowDao.SelectFlowList(PAGELIMIT, page, CompanySession.CompanyCode, 1, 2, 5);
            Dictionary<String, object> pRet = new Dictionary<String, object>();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].stateView(lType);
                pRet.Add("item" + i.ToString(), list[i]);
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", PAGELIMIT);

            return Json(pRet, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 리스트검색페이지
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ApprovePage(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApprovePage - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ApprovePage");
            LanguageType? lType = GetLanguageType();

            Dictionary<String, Object> sessionBuffer = new Dictionary<string, object>();
            ProductFlow flow = productFlowDao.SelectProductFlow(idx, CompanySession.CompanyCode);
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductBuyPrice * flow.ProductAmount;
            flow.stateView(lType);

            sessionBuffer.Add("FLOW", flow);
            if (flow.ApplyType != 0)
            {
                //발주서 검색
                OrderTable order = orderTableDao.SelectOrderTable(flow.ApplyType, CompanySession.CompanyCode);
                //정상경로가 아님
                if (order.Idx <= 0)
                {
                    return ErrorPage("/Home/Error");
                }
                sessionBuffer.Add("ORDER", order);
                //문서정보 취득
                Document pDoc = documentDao.SelectDocument(order.Idx, DOCUMENTSTORECODE, CompanySession.CompanyCode);
                sessionBuffer.Add("DOCUMENT", pDoc);
                //서버 상품 검색
                IList<OrderTableSub> orderSubList = orderTableSubDao.SelectSubList(order.Idx, CompanySession.CompanyCode, lType);
                sessionBuffer.Add("SUBLIST", orderSubList);
                //기본 데이터 Attribute저장
                ViewBag.doc = pDoc;
                ViewBag.order = order;
                ViewBag.orderSub = orderSubList;

                //전체 금액 계산
                ViewBag.totalMoney = orderSubList.Sum((pSub) => { return pSub.ProductMoney; });

                //코드마스터 취득
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
            }

            ViewBag.Commit = flow.State;
            Session[Define.Session.ACTION] = "ApplyList";
            return View("~/Views/Store/Web/ApplyView.cshtml", Define.MASTER_VIEW);
        }

        /*여기까지가 입고장처리*/

        /// <summary>
        /// 출고등록
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ReleaseAdd(String key)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseAdd - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseAdd");

            if (Object.Equals("BACK", key))
            {
                ProductFlow pFlow = (ProductFlow)Session["productFlow"];
                if (pFlow != null)
                {
                    ViewBag.Flow = pFlow;
                    ViewBag.totalprice = pFlow.ProductSellPrice * pFlow.ProductAmount;
                }
            }
            ViewBag.productlist = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            ViewBag.userInfo = UserSession;
            return View("~/Views/Store/Web/ReleaseAdd.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 출고등록입력확인
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleaseAddCheck(ProductFlow pFlow)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseAddCheck - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseAddCheck");
            LanguageType? lType = GetLanguageType();

            Session[Define.Session.ACTION] = "ReleaseAdd";
            List<String> pErrmsg = pFlow.Validate(lType);
            IList<ProductInfo> productList = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            //에러가 있는 경우
            if (pErrmsg.Count > 0)
            {
                String err = "";
                foreach (String pData in pErrmsg)
                {
                    err += pData + "<br>";
                }
                ViewBag.ErrMsg = err;
                ViewBag.Flow = pFlow;
                ViewBag.totalprice = pFlow.ProductSellPrice * pFlow.ProductAmount;
                ViewBag.productlist = productList;
                ViewBag.userInfo = UserSession;

                return View("~/Views/Store/Web/ReleaseAdd.cshtml", Define.MASTER_VIEW);
            }
            //정상의 경우
            foreach (ProductInfo pData in productList)
            {
                if (pData.Idx == pFlow.ProductIndex)
                {
                    pFlow.ProductName = pData.ProductName;
                    break;
                }
            }
            ViewBag.Flow = pFlow;
            ViewBag.totalprice = pFlow.ProductSellPrice * pFlow.ProductAmount;
            Session["productFlow"] = pFlow;

            ViewBag.Commit = COMMIT_KAKUNINN;
            return View("~/Views/Store/Web/ReleaseAddCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 출고등록입력
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleaseInsert()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseInsert - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseInsert");

            ProductFlow flow = (ProductFlow)Session["productFlow"];
            flow.State = Define.ProductFlow.OUTCOMESTANBY.ToString();
            flow.ApplyType = Define.ProductFlow.APPLYTYPE_NORMAL;
            flow.CompanyCode = CompanySession.CompanyCode;
            flow.ProductBuyPrice = 0; //입고초기화
            String pBuffer = flow.ProductName;
            flow.ProductName = null;
            productFlowDao.InsertProductFlow(flow);
            flow.ProductName = pBuffer;
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductSellPrice * flow.ProductAmount;

            //세션 초기화
            Session["productFlow"] = null;

            ViewBag.Commit = COMMIT_KANRYOU;
            Session[Define.Session.ACTION] = "ReleaseAdd";
            return View("~/Views/Store/Web/ReleaseAddCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 출고승인리스트
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ReleaseCheckList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseCheckList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseCheckList");
            ViewBag.list = productFlowDao.SelectFlow(PAGELIMIT, Define.PAGE_START, Define.ProductFlow.OUTCOMESTANBY, CompanySession.CompanyCode);
            ViewBag.listcount = Convert.ToInt32(Math.Ceiling((Double)((Double)productFlowDao.GetProductFlowCount(Define.ProductFlow.OUTCOMESTANBY, CompanySession.CompanyCode) / (Double)PAGELIMIT)));

            return View("~/Views/Store/Web/ReleaseCheckList.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 출고승인검색 AJAX
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleaseApproveSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseApproveSearch - NoAjax");
                return NoAjax();
            }
            int pagelimit = 50;
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseApproveSearch");
            if (page < 1)
            {
                page = 1;
            }
            int count = productFlowDao.GetProductFlowCount(Define.ProductFlow.OUTCOMESTANBY, CompanySession.CompanyCode);
            IList<ProductFlow> list = productFlowDao.SelectFlow(pagelimit, page, Define.ProductFlow.OUTCOMESTANBY, CompanySession.CompanyCode);
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
        /// 출고승인페이지
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleaseApprovePage(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseApprovePage - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseApprovePage");
            LanguageType? lType = GetLanguageType();

            Dictionary<String, Object> sessionBuffer = new Dictionary<string, object>();
            ProductFlow flow = productFlowDao.SelectProductFlow(idx, CompanySession.CompanyCode);
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductSellPrice * flow.ProductAmount;

            sessionBuffer.Add("FLOW", flow);
            if (flow.ApplyType != 0)
            {
                //발주서 검색
                OrderTable pOrder = orderTableDao.SelectOrderTable(flow.ApplyType, CompanySession.CompanyCode);

                //정상경로가 아님
                if (pOrder.Idx <= 0)
                {
                    return ErrorPage("/Home/Error");
                }
                sessionBuffer.Add("ORDER", pOrder);

                //문서정보 취득
                Document pDoc = documentDao.SelectDocument(pOrder.Idx, DOCUMENTSTORECODE, CompanySession.CompanyCode);
                sessionBuffer.Add("DOCUMENT", pDoc);

                //서버 상품 검색
                IList<OrderTableSub> orderSubList = orderTableSubDao.SelectSubList(pOrder.Idx, CompanySession.CompanyCode, lType);
                sessionBuffer.Add("SUBLIST", orderSubList);
                //기본 데이터 Attribute저장
                ViewBag.doc = pDoc;
                ViewBag.order = pOrder;
                ViewBag.orderSub = orderSubList;

                //전체 금액 계산
                ViewBag.totalMoney = orderSubList.Sum((pSub) => { return pSub.ProductMoney; });

                //코드마스터 취득
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
            }

            Session["ApproveData"] = sessionBuffer;
            ViewBag.Commit = COMMIT_SHOUNINNPAGE;
            Session[Define.Session.ACTION] = "ReleaseCheckList";
            return View("~/Views/Store/Web/ReleaseCheckPage.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 출고승인
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleaseApprove(String key)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseApprove - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseApprove");

            Dictionary<String, Object> sessionBuffer = (Dictionary<String, Object>)Session["ApproveData"];
            if (sessionBuffer == null)
            {
                return ErrorPage("/Home/Error");
            }
            else
            {
                Session["ApproveData"] = null;
            }

            LanguageType? lType = GetLanguageType();

            ProductFlow flow = (ProductFlow)sessionBuffer["FLOW"];
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductSellPrice * flow.ProductAmount;
            if (flow.ApplyType != 0)
            {
                OrderTable order = (OrderTable)sessionBuffer["ORDER"];
                Document doc = (Document)sessionBuffer["DOCUMENT"];
                IList<OrderTableSub> orderSubList = (IList<OrderTableSub>)sessionBuffer["SUBLIST"];
                //코드마스터 취득
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);

                //기본 데이터 Attribute저장
                ViewBag.doc = doc;
                ViewBag.order = order;
                ViewBag.orderSub = orderSubList;

                //전체 금액 계산
                ViewBag.totalMoney = orderSubList.Sum((pSub) => { return pSub.ProductMoney; }); ;
            }

            //등록부분
            if (Object.Equals(Define.APPROVE, key))
            {
                productFlowDao.ApproveProduct(flow.Idx, CompanySession.CompanyCode, Define.ProductFlow.OUTPUTCOMPLATE);
                Cargo cargo = new Cargo();
                cargo.ProductIndex = flow.ProductIndex;
                cargo.ProductInput = 0;
                cargo.ProductOutput = flow.ProductAmount;
                cargo.ProductMoney = flow.ProductAmount * flow.ProductSellPrice;
                cargo.Creater = UserSession.Creater;
                cargo.CreateDate = DateTime.Now;
                cargo.State = Define.STATE_NORMAL.ToString();
                cargo.CompanyCode = CompanySession.CompanyCode;
                cargoDao.InsertCargo(cargo);
                ViewBag.Commit = COMMIT_SHOUNINN;
            }
            else
            {
                productFlowDao.ApproveProduct(flow.Idx, CompanySession.CompanyCode, Define.ProductFlow.OUTPUTCANCEL);
                ViewBag.Commit = COMMIT_SHOUNINNCANCEL;
            }
            Session[Define.Session.ACTION] = "ReleaseCheckList";
            return View("~/Views/Store/Web/ReleaseCheckPage.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 출고 리스트(이력)
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ReleaseList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseList");

            LanguageType? lType = GetLanguageType();
            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)productFlowDao.CountFlowList(CompanySession.CompanyCode, Define.ProductFlow.OUTCOMESTANBY, Define.ProductFlow.OUTPUTCANCEL, Define.ProductFlow.OUTPUTCOMPLATE) / (Double)PAGELIMIT)));
            IList<ProductFlow> list = productFlowDao.SelectFlowList(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode, Define.ProductFlow.OUTCOMESTANBY, Define.ProductFlow.OUTPUTCANCEL, Define.ProductFlow.OUTPUTCOMPLATE);
            list.AsParallel().ForAll((pFlow) => { pFlow.stateView(lType); });
            ViewBag.list = list;
            ViewBag.listcount = count;

            return View("~/Views/Store/Web/ReleaseList.cshtml", Define.MASTER_VIEW);
        }
        //여기부터 수정
        /// <summary>
        /// 출고이력검색 AJAX
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleaseListSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseListSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleaseHistoryListSearch");
            LanguageType? lType = GetLanguageType();

            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = productFlowDao.CountFlowList(CompanySession.CompanyCode, Define.ProductFlow.OUTCOMESTANBY, Define.ProductFlow.OUTPUTCANCEL, Define.ProductFlow.OUTPUTCOMPLATE);
            IList<ProductFlow> list = productFlowDao.SelectFlowList(PAGELIMIT, page, CompanySession.CompanyCode, Define.ProductFlow.OUTCOMESTANBY, Define.ProductFlow.OUTPUTCANCEL, Define.ProductFlow.OUTPUTCOMPLATE);
            Dictionary<String, object> pRet = new Dictionary<String, object>();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].stateView(lType);
                pRet.Add("item" + i.ToString(), list[i]);
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", PAGELIMIT);

            return Json(pRet, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 리스트검색페이지출고
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ReleasePage(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleasePage - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/ReleasePage");
            LanguageType? lType = GetLanguageType();

            Dictionary<String, Object> sessionBuffer = new Dictionary<string, object>();
            ProductFlow flow = productFlowDao.SelectProductFlow(idx, CompanySession.CompanyCode);
            ViewBag.Flow = flow;
            ViewBag.totalprice = flow.ProductSellPrice * flow.ProductAmount;
            flow.stateView(lType);

            sessionBuffer.Add("FLOW", flow);
            if (flow.ApplyType != 0)
            {
                //수주서 검색
                OrderTable order = orderTableDao.SelectOrderTable(flow.ApplyType, CompanySession.CompanyCode);

                //정상경로가 아님
                if (order.Idx <= 0)
                {
                    return ErrorPage("/Home/Error");
                }
                sessionBuffer.Add("ORDER", order);
                //문서정보 취득
                Document doc = documentDao.SelectDocument(order.Idx, DOCUMENTSTORECODE, CompanySession.CompanyCode);
                sessionBuffer.Add("DOCUMENT", doc);
                //서버 상품 검색
                IList<OrderTableSub> orderSubList = orderTableSubDao.SelectSubList(order.Idx, CompanySession.CompanyCode, lType);
                sessionBuffer.Add("SUBLIST", orderSubList);
                //기본 데이터 Attribute저장
                ViewBag.doc = doc;
                ViewBag.order = order;
                ViewBag.orderSub = orderSubList;

                //전체 금액 계산
                Decimal totalMoney = 0;
                foreach (OrderTableSub pSub in orderSubList)
                {
                    totalMoney += pSub.ProductMoney;
                }
                ViewBag.totalMoney = totalMoney;

                //코드마스터 취득
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
            }

            //Session["ApproveData"] = sessionBuffer;
            //ViewBag.Commit = COMMIT_SHOUNINNPAGE;
            ViewBag.Commit = flow.State;
            Session[Define.Session.ACTION] = "ReleaseList";
            return View("~/Views/Store/Web/ReleaseView.cshtml", Define.MASTER_VIEW);
        }
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult StoreList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/StoreListn - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/StoreList");

            IList<Cargo> list = cargoDao.SelectCargoByCompanyCode(CompanySession.CompanyCode);
            ViewBag.list = list;
            ViewBag.listcount = list.Count;
            return View("~/Views/Store/Web/StoreList.cshtml", Define.MASTER_VIEW);
        }
        //*********************************

        /// <summary>
        /// 입출고수불
        /// </summary>
        /// <param name="pDoc"></param>
        /// <param name="pOrder"></param>
        /// <param name="pSubOrder"></param>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult StoreOrder()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/StoreOrder - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/StoreOrder");
            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)cargoDao.GetCargoListCount(CompanySession.CompanyCode) / (Double)PAGELIMIT)));
            IList<Cargo> list = cargoDao.SelectCargoList(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode);
            LanguageType? lType = GetLanguageType();
            list.AsParallel().ForAll((l) => { l.TypeCheck(lType); });
            ViewBag.list = list;
            ViewBag.listcount = count;

            return View("~/Views/Store/Web/StoreOrder.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 입출고수불검색 AJAX
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult StoreListSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/StoreListSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Store/StoreListSearch");
            if (page < 1)
            {
                page = 1;
            }
            int count = cargoDao.GetCargoListCount(CompanySession.CompanyCode);
            LanguageType? lType = GetLanguageType();
            IList<Cargo> list = cargoDao.SelectCargoList(PAGELIMIT, page, CompanySession.CompanyCode);
            Dictionary<String, object> pRet = new Dictionary<String, object>();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].TypeCheck(lType);
                pRet.Add("item" + i.ToString(), list[i]);
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", PAGELIMIT);

            return Json(pRet, JsonRequestBehavior.AllowGet);
        }
    }
}
