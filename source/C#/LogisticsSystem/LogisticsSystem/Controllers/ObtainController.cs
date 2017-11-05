using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogisticsSystem.Models;
using LogisticsSystem.App_Code;
using LogisticsSystem.Dao;
using LogisticsSystem.Master;
using LogisticsSystem.Validate;

namespace LogisticsSystem.Controllers
{
    public class ObtainController : AbstractController
    {
        private const int PAGELIMIT = 50;
        private const int ORDERCODE = 0;
        private const String DOCUMENTCODE = "1";
        private const int COMMIT_KANRYOU = 1;
        private const int COMMIT_KAKUNINN = 2;
        private const int COMMIT_SHOUNINN = 3;
        private const int COMMIT_SHOUNINN_COMPLATE = 4;
        private const int COMMIT_SHOUNINN_CANCLE = 5;
        private const String ORDERSESSIONKEY = "asdhfagshd";

        private CustomerInfoDao customerInfoDao = FactoryDao.Instance().GetCustomerInfoDao();
        private ProductInfoDao productInfoDao = FactoryDao.Instance().GetProductInfoDao();
        private DocumentDao documentDao = FactoryDao.Instance().GetDocumentDao();
        private OrderTableDao orderTableDao = FactoryDao.Instance().GetOrderTableDao();
        private OrderTableSubDao orderTableSubDao = FactoryDao.Instance().GetOrderTableSubDao();
        private ProductFlowDao productFlowDao = FactoryDao.Instance().GetProductFlowDao();

        /// <summary>
        /// 발주신청 초기
        /// </summary>
        /// <returns></returns>
        /// 확인에서 Cancel을 눌렀을때.
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult Order(String key = "")
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Order - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Order");
            LanguageType? lType = GetLanguageType();
            //회원정보
            ViewBag.userinfo = UserSession;
            //회사정보
            ViewBag.compinfo = CompanySession;
            ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);

            ViewBag.ordercomplist = customerInfoDao.SelectByOrderCompList(CompanySession.CompanyCode);
            ViewBag.productlist = productInfoDao.GetProductNameList(CompanySession.CompanyCode);

            Document doc = null;
            OrderTable order = null;

            if (Object.Equals("BACK", key))
            {
                doc = (Document)Session["orderDOC"];
                order = (OrderTable)Session["order"];
                IList<OrderTableSub> OrderSubList = (IList<OrderTableSub>)Session["orderSub"];
                if (doc == null || order == null)
                {
                    ViewBag.MunCode = documentDao.CreateCode();
                    ViewBag.BalCode = orderTableDao.CreateCode();
                    ViewBag.totalMoney = 0;
                }
                else
                {
                    ViewBag.MunCode = doc.DocumentCode;
                    ViewBag.BalCode = order.OrderNumber;
                    ViewBag.order = order;
                    ViewBag.orderSub = OrderSubList;
                    ViewBag.totalMoney = OrderSubList.Sum((pSub) => { return pSub.ProductMoney; });
                }
            }
            else
            {
                ViewBag.MunCode = documentDao.CreateCode();
                ViewBag.BalCode = orderTableDao.CreateCode();
                ViewBag.totalMoney = 0;
            }
            Session["orderDOC"] = null;
            Session["order"] = null;
            Session["orderSub"] = null;
            Session["orderCheck"] = null;

            return View("~/Views/Obtain/Web/Order.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 상품 셀렉트시 AJX
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ProductSelect(int idx)
        {
            //버그가 있음
            //에러가 있어서 재입력 할때 Cookie와 Session 값이 안 맞는 현상
            if (false && !SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ProductSelect - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ProductSelect");
            LanguageType? lType = GetLanguageType();
            ProductInfo product = productInfoDao.SelectProduct(idx, CompanySession.CompanyCode, lType);
            return Json(product, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 입력확인
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult Inputcheck(Document pDoc, OrderTable pOrder, FormRequestOrderTableList pSubOrder, int inordernameIdx = 0)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Inputcheck - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Inputcheck");
            LanguageType? lType = GetLanguageType();
            //잘못된 경로로 들어올 시
            if (String.IsNullOrEmpty(pDoc.DocumentCode))
            {
                return Redirect("/Obtain/Order");
            }
            if (inordernameIdx != 0)
            {
                pOrder.InorderName = customerInfoDao.SelectCustomer(inordernameIdx, CompanySession.CompanyCode).CustomerName;
            }
            List<String> pErrmsg = new List<string>();
            pErrmsg.AddRange(pDoc.Validate(lType));
            pErrmsg.AddRange(pOrder.Validate(lType));
            pErrmsg.AddRange(pSubOrder.Validate(lType));
            Session[Define.Session.ACTION] = "Order";
            //에러가 발생시
            if (pErrmsg.Count > 0)
            {
                //******************************************************
                //회원정보
                ViewBag.userinfo = UserSession;
                //회사정보
                ViewBag.compinfo = CompanySession;
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
                ViewBag.ordercomplist = customerInfoDao.SelectByOrderCompList(CompanySession.CompanyCode);
                ViewBag.productlist = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
                ViewBag.MunCode = pDoc.DocumentCode;
                ViewBag.BalCode = pOrder.OrderNumber;
                ViewBag.order = pOrder;
                ViewBag.orderSub = pSubOrder.List;

                String err = "";
                foreach (String pData in pErrmsg)
                {
                    err += pData + "<br>";
                }
                ViewBag.ErrMsg = err;
                ViewBag.totalMoney = pSubOrder.List.Sum((pSub) => { return pSub.ProductMoney; });
                return View("~/Views/Obtain/Web/Order.cshtml", Define.MASTER_VIEW);
            }

            //정상시
            Document doc = pDoc;
            OrderTable order = pOrder;
            IList<OrderTableSub> orderSub = pSubOrder.List;

            //세션 저장
            Session["orderDOC"] = doc;
            Session["order"] = order;
            Session["orderSub"] = orderSub;
            Session["orderCheck"] = ORDERSESSIONKEY;

            //상품정보 취득
            IList<ProductInfo> productList = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            orderSub.AsParallel().ForAll((data) =>
            {
                IList<ProductInfo> product = (from info in productList where Object.Equals(info.Idx, data.ProductIndex) select info).ToList();
                if (product.Count() > 0)
                {
                    data.ProductName = product[0].ProductName;
                }
            });
            //기본ViewBag 설정
            ViewSetting(doc, order, orderSub);
            ViewBag.Commit = COMMIT_KAKUNINN;
            return View("~/Views/Obtain/Web/OrderCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 승인시 저장
        /// </summary>
        /// <param name="pDoc"></param>
        /// <param name="pOrder"></param>
        /// <param name="pSubOrder"></param>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Input()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Input - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Input");

            Document doc = (Document)Session["orderDOC"];
            OrderTable order = (OrderTable)Session["order"];
            IList<OrderTableSub> orderSubList = (IList<OrderTableSub>)Session["orderSub"];
            String InputSessionkey = (String)Session["orderCheck"];
            //비정상경로
            if (doc == null || order == null || orderSubList == null || InputSessionkey == null)
            {
                return Redirect("/Obtain/Order");
            }

            //비정상경로
            if (!ORDERSESSIONKEY.Equals(InputSessionkey))
            {
                return Redirect("/Obtain/Order");
            }
            Session["orderDOC"] = null;
            Session["order"] = null;
            Session["orderSub"] = null;
            Session["orderCheck"] = null;

            //************* 발주 테이블
            order.State = App_Code.Define.STATE_NORMAL.ToString();
            order.OrderType = ORDERCODE.ToString();
            order.CompanyCode = CompanySession.CompanyCode;
            //발주서정보저장
            orderTableDao.InsertOrder(order);

            //*************문서테이블
            doc.DocumentIndex = order.Idx;
            doc.CompanyCode = CompanySession.CompanyCode;
            doc.DocumentType = DOCUMENTCODE;    //발주서 코드
            doc.State = App_Code.Define.STATE_NORMAL.ToString();
            //문서정보저장
            documentDao.InsertDocument(doc);

            //상품테이블
            //서버 상품정보 저장
            orderTableSubDao.InsertOrderSubList(orderSubList, order.Idx, doc.Creater, CompanySession.CompanyCode);
            ViewBag.Commit = COMMIT_KANRYOU;

            //기본ViewBag 설정
            ViewSetting(doc, order, orderSubList);
            IList<ProductInfo> productList = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            orderSubList.AsParallel().ForAll((data) =>
            {
                IList<ProductInfo> product = (from info in productList where Object.Equals(info.Idx, data.ProductIndex) select info).ToList();
                if (product.Count() > 0)
                {
                    data.ProductName = product[0].ProductName;
                }
            });
            Session[Define.Session.ACTION] = "Order";
            return View("~/Views/Obtain/Web/OrderCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 발주승인페이지리스트
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult OrderApproveList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/OrderApproveList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/OrderApproveList");
            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, true) / (Double)PAGELIMIT)));
            IList<OrderTable> list = orderTableDao.SelectOrder(PAGELIMIT, Define.PAGE_START, ORDERCODE, CompanySession.CompanyCode, true);
            String idxcollection = "";
            foreach (OrderTable pBuffer in list)
            {
                idxcollection += pBuffer.Idx.ToString() + ",";
            }
            ViewBag.list = list;
            ViewBag.listcount = count;
            ViewBag.idxCollection = idxcollection;

            return View("~/Views/Obtain/Web/OrderApproveList.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 발주승인페이지에서 검색(AJAX)
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ListCheckSearch(int page = 0)
        {
            //Idx Collection 신경써야한다.
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ListCheckSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ListCheckSearch");
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }

            int count = orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, true);
            IList<OrderTable> list = orderTableDao.SelectOrder(PAGELIMIT, page, ORDERCODE, CompanySession.CompanyCode, true);
            Dictionary<String, object> pRet = new Dictionary<String, object>();
            for (int i = 0; i < list.Count; i++)
            {
                pRet.Add("item" + i.ToString(), list[i]);
            }
            String idxcollection = "";
            foreach (OrderTable pBuffer in list)
            {
                idxcollection += pBuffer.Idx.ToString() + ",";
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", PAGELIMIT);
            pRet.Add("idxcollection", idxcollection);
            return Json(pRet, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 세부검색 - 항목을 클릭할 시에 나오는 Ajax
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult SubSearch(int idx)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/SubSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/SubSearch");
            LanguageType? lType = GetLanguageType();
            IList<OrderTableSub> list = orderTableSubDao.SelectSubList(idx, CompanySession.CompanyCode, lType);
            Dictionary<String, object> pRet = new Dictionary<String, object>();
            for (int i = 0; i < list.Count; i++)
            {
                pRet.Add("item" + i.ToString(), list[i]);
            }
            pRet.Add("count", list.Count);
            return Json(pRet, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 승인페이지
        /// </summary>
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ApprovePage - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ApprovePage");
            LanguageType? lType = GetLanguageType();
            //정상경로가 아님
            if (idx < Define.PAGE_START)
            {
                return ErrorPage("/Home/Error");
            }

            OrderTable order = orderTableDao.SelectOrderTable(idx, CompanySession.CompanyCode);
            //발주서 검색
            //정상경로가 아님
            if (order.Idx <= 0)
            {
                return ErrorPage("/Home/Error");
            }
            //문서정보 취득
            Document doc = documentDao.SelectDocument(order.Idx, DOCUMENTCODE, CompanySession.CompanyCode);

            //서버 상품 검색
            IList<OrderTableSub> orderSubList = orderTableSubDao.SelectSubList(idx, CompanySession.CompanyCode, lType);

            //기본ViewBag 설정
            ViewSetting(doc, order, orderSubList);
            //세션 저장
            Session["orderDOC"] = doc;
            Session["order"] = order;
            Session["orderSub"] = orderSubList;
            Session["orderCheck"] = ORDERSESSIONKEY;

            ViewBag.Commit = COMMIT_SHOUNINN;

            Session[Define.Session.ACTION] = "OrderApproveList";
            return View("~/Views/Obtain/Web/OrderApproveCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 승인처리
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Approve(string key = "")
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Approve - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Approve");
            //정상경로가 아님
            if (!String.IsNullOrEmpty(key) && !String.Equals(Define.APPROVE, key) && !String.Equals(Define.CANCEL, key))
            {
                return ErrorPage("/Home/Error");
            }
            Document doc = (Document)Session["orderDOC"];
            OrderTable order = (OrderTable)Session["order"];
            IList<OrderTableSub> orderSubList = (IList<OrderTableSub>)Session["orderSub"];
            String InputSessionkey = (String)Session["orderCheck"];
            //비정상경로
            if (doc == null || order == null || orderSubList == null || InputSessionkey == null)
            {
                return Redirect("/Obtain/Order");
            }

            //비정상경로
            if (!ORDERSESSIONKEY.Equals(InputSessionkey))
            {
                return Redirect("/Obtain/Order");
            }
            Session["orderDOC"] = null;
            Session["order"] = null;
            Session["orderSub"] = null;
            Session["orderCheck"] = null;

            //등록처리
            if (String.Equals(Define.APPROVE, key))
            {
                //등록
                orderTableDao.Approve(order.Idx, App_Code.Define.STATE_APPLY, CompanySession.CompanyCode);
                foreach (OrderTableSub data in orderSubList)
                {
                    //재고등록
                    ProductFlow productFlow = new ProductFlow();
                    productFlow.ProductIndex = data.ProductIndex;
                    productFlow.ProductAmount = data.ProductAmount;
                    productFlow.ProductBuyPrice = data.ProductPrice;
                    productFlow.ProductSellPrice = 0;
                    productFlow.State = Define.ProductFlow.INCOMESTANBY.ToString();
                    productFlow.CompanyCode = CompanySession.CompanyCode;
                    productFlow.ApplyType = order.Idx;
                    productFlow.Creater = UserSession.UserId;
                    productFlow.CreteDate = DateTime.Now;
                    productFlowDao.InsertProductFlow(productFlow);
                    //등록
                    orderTableSubDao.ModifyState(data.Idx, App_Code.Define.STATE_APPLY, CompanySession.CompanyCode);
                }
                ViewBag.Commit = COMMIT_SHOUNINN_COMPLATE;

                //기본ViewBag 설정
                ViewSetting(doc, order, orderSubList);

                Session[Define.Session.ACTION] = "OrderApproveList";
                return View("~/Views/Obtain/Web/OrderApproveCheck.cshtml", Define.MASTER_VIEW);
            }
            //승인취소
            else if (String.Equals(Define.CANCEL, key))
            {
                //등록
                orderTableDao.Approve(order.Idx, App_Code.Define.STATE_DELETE, CompanySession.CompanyCode);
                foreach (OrderTableSub data in orderSubList)
                {
                    //등록
                    orderTableSubDao.ModifyState(data.Idx, App_Code.Define.STATE_DELETE, CompanySession.CompanyCode);
                }

                ViewBag.Commit = COMMIT_SHOUNINN_CANCLE;

                //기본ViewBag 설정
                ViewSetting(doc, order, orderSubList);

                Session[Define.Session.ACTION] = "OrderApproveList";
                return View("~/Views/Obtain/Web/OrderApproveCheck.cshtml", Define.MASTER_VIEW);
            }
            else
            {
                return ErrorPage("/Home/Error");
            }

        }

        /// <summary>
        /// 발주리스트(이력까지 포함)
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult OrderList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/OrderList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ObtainList");
            LanguageType? lType = GetLanguageType();

            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, false) / (Double)PAGELIMIT)));
            IList<OrderTable> list = orderTableDao.SelectOrder(PAGELIMIT, Define.PAGE_START, ORDERCODE, CompanySession.CompanyCode, false);

            String idxcollection = "";
            foreach (OrderTable pBuffer in list)
            {
                pBuffer.StateView(lType);
                idxcollection += pBuffer.Idx.ToString() + ",";
            }
            ViewBag.list = list;
            ViewBag.listcount = count;
            ViewBag.idxCollection = idxcollection;

            return View("~/Views/Obtain/Web/OrderList.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 발주리스트에서 검색 (AJAX 용)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ListSearch(int page = 0)
        {
            //Idx Collection 신경써야한다.
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Product/ListCheckSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ListSearch");
            LanguageType? lType = GetLanguageType();

            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, false);
            IList<OrderTable> list = orderTableDao.SelectOrder(PAGELIMIT, page, ORDERCODE, CompanySession.CompanyCode, false);
            Dictionary<String, object> pRet = new Dictionary<String, object>();
            for (int i = 0; i < list.Count; i++)
            {
                pRet.Add("item" + i.ToString(), list[i]);
            }
            String idxcollection = "";

            foreach (OrderTable pBuffer in list)
            {
                //상태
                pBuffer.StateView(lType);
                idxcollection += pBuffer.Idx.ToString() + ",";
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", PAGELIMIT);
            pRet.Add("idxcollection", idxcollection);
            return Json(pRet, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 기본 Attribute 세팅
        /// </summary>
        protected void ViewSetting(Document pDoc, OrderTable pOrder, IList<OrderTableSub> pSubOrder)
        {
            LanguageType? lType = GetLanguageType();

            //기본 데이터 Attribute저장
            ViewBag.doc = pDoc;
            ViewBag.order = pOrder;
            ViewBag.orderSub = pSubOrder;

            //전체 금액 계산
            ViewBag.totalMoney = pSubOrder.Sum((pSub) => { return pSub.ProductMoney; });

            //코드마스터 취득
            ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster("MoneySendType", lType);
        }
        /// <summary>
        /// 발주서 열람처리
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult OrderView(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/OrderView - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/OrderView");
            Session[Define.Session.ACTION] = "OrderList";
            LanguageType? lType = GetLanguageType();
            //정상경로가 아님
            if (idx < 1)
            {
                return ErrorPage("/Home/Error");
            }
            //발주서 검색
            OrderTable order = orderTableDao.SelectOrderTable(idx, CompanySession.CompanyCode);
            //정상경로가 아님
            if (order.Idx <= 0)
            {
                return ErrorPage("/Home/Error");
            }
            //문서정보 취득
            Document doc = documentDao.SelectDocument(order.Idx, DOCUMENTCODE, CompanySession.CompanyCode);

            //서버 상품 검색
            IList<OrderTableSub> orderSubList = orderTableSubDao.SelectSubList(idx, CompanySession.CompanyCode, lType);

            //기본ViewBag 설정
            ViewSetting(doc, order, orderSubList);
            //세션 저장
            Session["orderDOC"] = doc;
            Session["order"] = order;
            Session["orderSub"] = orderSubList;
            Session["orderIdx"] = idx;

            ViewBag.Commit = order.State;
            Session[Define.Session.ACTION] = "OrderList";
            return View("~/Views/Obtain/Web/OrderView.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// PDF 출력
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult PDFCreate()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/PDFCreate - Error");
                return base.Logout();
            }

            if (Session["orderIdx"] == null)
            {
                return ErrorPage("/Home/Error");
            }
            Dictionary<String, Object> parameter = new Dictionary<string, object>();
            Document doc = (Document)Session["orderDOC"];
            OrderTable order = (OrderTable)Session["order"];
            IList<OrderTableSub> orderSubList = (IList<OrderTableSub>)Session["orderSub"];

            //비정상 경로
            if (doc == null || order == null || orderSubList == null)
            {
                return Redirect("/Obtain/Order");
            }

            LanguageType? lType = String.Equals("1", order.PrintSetting) ? LanguageType.Korea : LanguageType.Japan;
            LanguagePack pLang = new LanguagePack(Server.MapPath("~/Language"), "PDF", "Order", lType);

            decimal totalMoney = 0;

            if (!Object.Equals(lType, LanguageType.Japan))
            {
                parameter.Add("companytitle", CompanySession.CompanyName);
            }
            else
            {
                parameter.Add("companytitle", CompanySession.CompanyName_En);
            }

            parameter.Add("ordernowdate", DateTime.Now.ToString("yyyy-MM-dd"));
            parameter.Add("DocumentNumber_result", doc.DocumentCode);
            parameter.Add("CreateDate_result", doc.CreateDate.ToString("yyyy-MM-dd"));
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = doc.Creater_En;
                parameter.Add("Creater_result", buf);
            }
            else
            {
                parameter.Add("Creater_result", doc.Creater);
            }
            parameter.Add("OrderNumber_result", order.OrderNumber);
            parameter.Add("DeliveryComp_result", order.InorderName);
            parameter.Add("DeliveryAddress_result", order.InOrderAddress);
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                parameter.Add("Totalmoney_result", order.OrderMoney.ToString("###,##0") + "엔");
            }
            else
            {
                parameter.Add("Totalmoney_result", order.OrderMoney.ToString("###,##0") + "円");
            }
            parameter.Add("PeriodDate_result", order.OrderSaveDate.ToString("yyyy-MM-dd"));
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = order.OrderSavePlace_En;
                parameter.Add("PeriodPlace_result", buf);
            }
            else
            {
                parameter.Add("PeriodPlace_result", order.OrderSavePlace);
            }
            parameter.Add("ordername_result", order.OrderName);
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = order.OrderAddress_En;
                parameter.Add("CompAddress_result", buf);
            }
            else
            {
                parameter.Add("CompAddress_result", order.OrderAddress);
            }
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = "+81-" + order.OrderPhoneNumber.Substring(1);
                parameter.Add("CompNumber_result", buf);
            }
            else
            {
                parameter.Add("CompNumber_result", order.OrderPhoneNumber);
            }
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = "+81-" + order.OrderFax.Substring(1);
                parameter.Add("CompFax_result", buf);
            }
            else
            {
                parameter.Add("CompFax_result", order.OrderFax);
            }
            parameter.Add("MoneyDate_result", order.PayDate.ToString("yyyy-MM-dd"));
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                parameter.Add("Money_result", order.PayMoney.ToString("###,##0") + "엔");
            }
            else
            {
                parameter.Add("Money_result", order.PayMoney.ToString("###,##0") + "円");
            }
            IList<CodeMaster> codeList = CodeMasterMap.Instance().GetCodeMaster("MoneySendType", lType);
            foreach (CodeMaster cm in codeList)
            {
                if (cm.CodeKey.Equals(order.PayCondition))
                {
                    parameter.Add("Moneycheck_result", cm.CodeName);
                }
            }
            parameter.Add("MoneyOrderDate_result", order.OrderDate.ToString("yyyy-MM-dd"));
            for (int i = 1; i <= 15; i++)
            {
                if (orderSubList.Count >= i)
                {
                    parameter.Add("ListNumber" + i.ToString(), i.ToString());
                    parameter.Add("ListProductName" + i.ToString(), orderSubList[i - 1].ProductName);
                    parameter.Add("ListProductSpec" + i.ToString(), orderSubList[i - 1].ProductSpec);
                    parameter.Add("ListMount" + i.ToString(), orderSubList[i - 1].ProductAmount.ToString("###,##0"));
                    if (!Object.Equals(lType, LanguageType.Japan))
                    {
                        parameter.Add("ListMoney" + i.ToString(), orderSubList[i - 1].ProductPrice.ToString("###,##0") + "엔");
                        parameter.Add("ListTotalMoney" + i.ToString(), orderSubList[i - 1].ProductMoney.ToString("###,##0") + "엔");
                    }
                    else
                    {
                        parameter.Add("ListMoney" + i.ToString(), orderSubList[i - 1].ProductPrice.ToString("###,##0") + "円");
                        parameter.Add("ListTotalMoney" + i.ToString(), orderSubList[i - 1].ProductMoney.ToString("###,##0") + "円");
                    }
                    totalMoney += orderSubList[i - 1].ProductMoney;
                }
                else
                {
                    parameter.Add("ListNumber" + i.ToString(), i.ToString());
                    parameter.Add("ListProductName" + i.ToString(), "");
                    parameter.Add("ListProductSpec" + i.ToString(), "");
                    parameter.Add("ListMount" + i.ToString(), "");
                    parameter.Add("ListMoney" + i.ToString(), "");
                    parameter.Add("ListTotalMoney" + i.ToString(), "");
                }
            }
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                parameter.Add("ListTotalSumMoney_result", totalMoney.ToString("###,##0") + "엔");
            }
            else
            {
                parameter.Add("ListTotalSumMoney_result", totalMoney.ToString("###,##0") + "円");
            }

            parameter.Add("Memo_result", order.PayOther);

            foreach (string key in pLang.Keys)
            {
                if (!parameter.ContainsKey(key))
                {
                    parameter.Add(key, pLang[key]);
                }
            }
            using (App_Code.PDFCreate pdf = new PDFCreate())
            {
                pdf.Open();
                System.IO.MemoryStream stm = (System.IO.MemoryStream)pdf.PdfCreater(Server.MapPath("..\\PDF\\OrderForm.html"), parameter);
                pdf.Close();
                String filename = "Order_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";
                return File(stm.GetBuffer(), "application/octet-stream", filename);
            }
        }
        /// <summary>
        /// PDF 출력
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult PDFPrint(String language, String engcheck)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/PDFCreate - Error");
                return base.Logout();
            }
            Dictionary<String, Object> parameter = new Dictionary<string, object>();

            Document doc = (Document)Session["orderDOC"];
            OrderTable order = (OrderTable)Session["order"];
            IList<OrderTableSub> orderSubList = (IList<OrderTableSub>)Session["orderSub"];

            LanguageType? lType = String.Equals("korean", language) ? LanguageType.Korea : LanguageType.Japan;
            LanguagePack pLang = new LanguagePack(Server.MapPath("~/Language"), "PDF", "Order", lType);

            decimal totalMoney = 0;

            parameter.Add("companytitle", CompanySession.CompanyName);
            parameter.Add("companytitle_en", CompanySession.CompanyName_En);
            parameter.Add("ordernowdate", DateTime.Now.ToString("yyyy-MM-dd"));
            parameter.Add("DocumentNumber_result", doc.DocumentCode);
            parameter.Add("CreateDate_result", doc.CreateDateString);
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = doc.Creater + "<br>" + doc.Creater_En;
                parameter.Add("Creater_result", buf);
            }
            else
            {
                parameter.Add("Creater_result", doc.Creater);
            }
            parameter.Add("OrderNumber_result", order.OrderNumber);
            parameter.Add("DeliveryComp_result", order.InorderName);
            parameter.Add("Totalmoney_result", order.OrderMoney.ToString("###,##0"));
            parameter.Add("PeriodDate_result", order.OrderSaveDate.ToString("yyyy-MM-dd"));

            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = order.OrderSavePlace + "<br>" + order.OrderSavePlace_En;
                parameter.Add("PeriodPlace_result", buf);
            }
            else
            {
                parameter.Add("PeriodPlace_result", order.OrderSavePlace);
            }
            parameter.Add("ordername_result", order.OrderName);

            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = order.OrderAddress + "<br>" + order.OrderAddress_En;
                parameter.Add("CompAddress_result", buf);
            }
            else
            {
                parameter.Add("CompAddress_result", order.OrderAddress);
            }

            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = "+81-" + order.OrderPhoneNumber.Substring(1);
                parameter.Add("CompNumber_result", buf);
            }
            else
            {
                parameter.Add("CompNumber_result", order.OrderPhoneNumber);
            }
            if (!Object.Equals(lType, LanguageType.Japan))
            {
                String buf = "+81-" + order.OrderFax.Substring(1);
                parameter.Add("CompFax_result", buf);
            }
            else
            {
                parameter.Add("CompFax_result", order.OrderFax);
            }
            parameter.Add("MoneyDate_result", order.PayDate.ToString("yyyy-MM-dd"));
            parameter.Add("Money_result", order.PayMoney.ToString("###,##0"));

            IList<CodeMaster> codeList = CodeMasterMap.Instance().GetCodeMaster("MoneySendType", lType);
            foreach (CodeMaster cm in codeList)
            {
                if (cm.CodeKey.Equals(order.PayCondition))
                {
                    parameter.Add("Moneycheck_result", cm.CodeName);
                }
            }
            parameter.Add("MoneyOrderDate_result", order.OrderDate.ToString("yyyy-MM-dd"));

            for (int i = 1; i <= 15; i++)
            {
                if (orderSubList.Count >= i)
                {
                    parameter.Add("ListNumber" + i.ToString(), i.ToString());
                    parameter.Add("ListProductName" + i.ToString(), orderSubList[i - 1].ProductName);
                    parameter.Add("ListProductSpec" + i.ToString(), orderSubList[i - 1].ProductSpecDisp);
                    parameter.Add("ListMount" + i.ToString(), orderSubList[i - 1].ProductAmount.ToString("###,##0"));
                    parameter.Add("ListMoney" + i.ToString(), orderSubList[i - 1].ProductPrice.ToString("###,##0"));
                    parameter.Add("ListTotalMoney" + i.ToString(), orderSubList[i - 1].ProductMoney.ToString("###,##0"));
                    totalMoney += orderSubList[i - 1].ProductMoney;
                }
                else
                {
                    parameter.Add("ListNumber" + i.ToString(), i.ToString());
                    parameter.Add("ListProductName" + i.ToString(), "");
                    parameter.Add("ListProductSpec" + i.ToString(), "");
                    parameter.Add("ListMount" + i.ToString(), "");
                    parameter.Add("ListMoney" + i.ToString(), "");
                    parameter.Add("ListTotalMoney" + i.ToString(), "");
                }
            }

            parameter.Add("ListTotalSumMoney_result", totalMoney.ToString("###,##0"));

            foreach (string key in pLang.Keys)
            {
                if (!parameter.ContainsKey(key))
                    parameter.Add(key, pLang[key]);
            }
            using (App_Code.PDFCreate pdf = new PDFCreate())
            {
                pdf.Open();
                System.IO.MemoryStream stm = (System.IO.MemoryStream)pdf.PdfCreater(Server.MapPath("..\\PDF\\OrderForm.html"), parameter);
                pdf.Close();
                String filename = "Order_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";
                return File(stm.GetBuffer(), "application/octet-stream", filename);
            }
        }
        /// <summary>
        /// 업체 셀렉트시 AJX
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult CompanySelect(int idx)
        {
            //버그가 있음
            //에러가 있어서 재입력 할때 Cookie와 Session 값이 안 맞는 현상
            if (false && !SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ProductSelect - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/CompanySelect");
            CustomerInfo custInfo = customerInfoDao.SelectCustomer(idx, CompanySession.CompanyCode);
            return Json(custInfo, JsonRequestBehavior.AllowGet);
        }
    }
}
