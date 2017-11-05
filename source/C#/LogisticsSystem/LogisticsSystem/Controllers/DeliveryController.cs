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
    public class DeliveryController : AbstractController
    {
        private const int PAGELIMIT = 50;
        private const int ORDERCODE = 1;
        private const String DOCUMENTCODE = "1";
        private const String DOCUMENTDELIVERYCODE = "2";
        private const String DOCUMENTBILLCODE = "3";
        private const int COMMIT_KANRYOU = 1;
        private const int COMMIT_KAKUNINN = 2;
        private const int COMMIT_SHOUNINN = 3;
        private const int COMMIT_SHOUNINN_COMPLATE = 4;
        private const int COMMIT_SHOUNINN_CANCLE = 5;

        private const String ORDERSESSIONKEY = "ijdfbfoasbfdka";

        private DocumentDao documentDao = FactoryDao.Instance().GetDocumentDao();
        private CustomerInfoDao customerInfoDao = FactoryDao.Instance().GetCustomerInfoDao();
        private ProductInfoDao productInfoDao = FactoryDao.Instance().GetProductInfoDao();
        private OrderTableDao orderTableDao = FactoryDao.Instance().GetOrderTableDao();
        private OrderTableSubDao orderTableSubDao = FactoryDao.Instance().GetOrderTableSubDao();
        private DeliveryTableDao deliveryTableDao = FactoryDao.Instance().GetDeliveryTableDao();
        private ProductFlowDao productFlowDao = FactoryDao.Instance().GetProductFlowDao();
        private DeliveryTableSubDao deliveryTableSubDao = FactoryDao.Instance().GetDeliveryTableSubDao();
        private BillDao billDao = FactoryDao.Instance().GetBillDao();

        /// <summary>
        /// 수주신청서 등록
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult DeliveryOrder(String key = "")
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryOrder");
            LanguageType? lType = GetLanguageType();

            ViewBag.userinfo = UserSession;
            ViewBag.compinfo = CompanySession;
            ViewBag.ordercomplist = customerInfoDao.SelectByInorderCompList(CompanySession.CompanyCode);
            ViewBag.productlist = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);

            if (Object.Equals("BACK", key))
            {
                Document doc = (Document)Session["orderDOC"];
                OrderTable order = (OrderTable)Session["order"];
                IList<OrderTableSub> orderList = (IList<OrderTableSub>)Session["orderSub"];
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
                    ViewBag.orderSub = orderList;
                    ViewBag.totalMoney = orderList.Sum((pSub) => { return pSub.ProductMoney; });
                }
            }
            else
            {
                ViewBag.MunCode = documentDao.CreateCode();
                ViewBag.BalCode = orderTableDao.CreateCode();
                ViewBag.totalMoney = 0;
                ViewBag.totalMoney = 0;
            }
            Session["orderDOC"] = null;
            Session["order"] = null;
            Session["orderSub"] = null;
            Session["orderCheck"] = null;

            return View("~/Views/Delivery/Web/DeliveryOrder.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 수주신청폼에서 회사 정보 취득 AJAX
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult orderSelect(int idx)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/orderSelect - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/orderSelect");
            CustomerInfo customer = customerInfoDao.SelectCustomer(idx, CompanySession.CompanyCode);
            return Json(customer, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 입력확인
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult Inputcheck(Document doc, OrderTable order, FormRequestOrderTableList subOrder)
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
            Session[Define.Session.ACTION] = "DeliveryOrder";
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/Inputcheck");
            LanguageType? lType = GetLanguageType();
            //잘못된 경로로 들어올 시
            if (String.IsNullOrEmpty(doc.DocumentCode))
            {
                return Redirect("/Delivery/DeliveryOrder");
            }

            List<String> pErrmsg = new List<string>();
            pErrmsg.AddRange(doc.Validate(lType));
            pErrmsg.AddRange(order.Validate(lType));
            pErrmsg.AddRange(subOrder.Validate(lType));

            //에러가 발생시
            if (pErrmsg.Count > 0)
            {
                ViewBag.userinfo = UserSession;
                ViewBag.compinfo = CompanySession;
                ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
                ViewBag.ordercomplist = customerInfoDao.SelectByInorderCompList(CompanySession.CompanyCode);
                ViewBag.productlist = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
                ViewBag.MunCode = doc.DocumentCode;
                ViewBag.BalCode = order.OrderNumber;
                ViewBag.order = order;
                ViewBag.orderSub = subOrder.List;

                String err = "";
                foreach (String pData in pErrmsg)
                {
                    err += pData + "<br>";
                }
                ViewBag.ErrMsg = err;
                ViewBag.totalMoney = subOrder.List.Sum((pSub) => { return pSub.ProductMoney; });
                return View("~/Views/Delivery/Web/DeliveryOrder.cshtml", Define.MASTER_VIEW);
            }
            //정상시
            else
            {
                IList<OrderTableSub> orderSub = subOrder.List;

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
                return View("~/Views/Delivery/Web/DeliveryOrderCheck.cshtml", Define.MASTER_VIEW);
            }

        }
        /// <summary>
        /// 수주신청입력 Action
        /// </summary>
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/Input - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/Input");

            Document doc = (Document)Session["orderDOC"];
            OrderTable order = (OrderTable)Session["order"];
            IList<OrderTableSub> suborderList = (IList<OrderTableSub>)Session["orderSub"];
            String InputSessionkey = (String)Session["orderCheck"];
            Session["orderDOC"] = null;
            Session["order"] = null;
            Session["orderSub"] = null;
            Session["orderCheck"] = null;

            //비정상경로
            if (doc == null || order == null || suborderList == null || InputSessionkey == null)
            {
                return Redirect("/Obtain/Order");
            }

            //비정상경로
            if (!ORDERSESSIONKEY.Equals(InputSessionkey))
            {
                return Redirect("/Obtain/Order");
            }

            //************* 발주 테이블
            order.State = App_Code.Define.STATE_NORMAL.ToString();
            order.OrderType = ORDERCODE.ToString();
            order.CompanyCode = CompanySession.CompanyCode;
            //발주서정보저장
            orderTableDao.InsertOrder(order);

            //*************문서테이블
            doc.DocumentIndex = order.Idx;
            doc.CompanyCode = CompanySession.CompanyCode;
            doc.DocumentType = DOCUMENTCODE;
            doc.State = App_Code.Define.STATE_NORMAL.ToString();
            //문서정보저장
            documentDao.InsertDocument(doc);

            //상품테이블
            //서버 상품정보 저장
            orderTableSubDao.InsertOrderSubList(suborderList, order.Idx, doc.Creater, CompanySession.CompanyCode);
            ViewBag.Commit = COMMIT_KANRYOU;

            //기본ViewBag 설정
            ViewSetting(doc, order, suborderList);

            IList<ProductInfo> productList = productInfoDao.GetProductNameList(CompanySession.CompanyCode);
            suborderList.AsParallel().ForAll((data) =>
            {
                IList<ProductInfo> product = (from info in productList where Object.Equals(info.Idx, data.ProductIndex) select info).ToList();
                if (product.Count() > 0)
                {
                    data.ProductName = product[0].ProductName;
                }
            });
            Session[Define.Session.ACTION] = "DeliveryOrder";
            return View("~/Views/Delivery/Web/DeliveryOrderCheck.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 기본 Attribute 세팅
        /// </summary>
        protected void ViewSetting(Document doc, OrderTable order, IList<OrderTableSub> subOrder)
        {
            LanguageType? lType = GetLanguageType();

            //기본 데이터 Attribute저장
            ViewBag.doc = doc;
            ViewBag.order = order;
            ViewBag.orderSub = subOrder;

            //전체 금액 계산
            ViewBag.totalMoney = subOrder.Sum((pSub) => { return pSub.ProductMoney; });

            //코드마스터 취득
            ViewBag.moneySendType = CodeMasterMap.Instance().GetCodeMaster(Define.CodeMaster.MONEY_SEND_TYPE, lType);
        }
        /// <summary>
        /// 수주승인리스트
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult DeliveryApproveList()
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
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryApproveList");
            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, true) / (Double)PAGELIMIT)));
            IList<OrderTable> list = orderTableDao.SelectOrder(PAGELIMIT, Define.PAGE_START, ORDERCODE, CompanySession.CompanyCode, true);
            String idxcollection = "";
            foreach (OrderTable l in list)
            {
                idxcollection += l.Idx.ToString() + ",";
            }
            ViewBag.list = list;
            ViewBag.listcount = count;
            ViewBag.idxCollection = idxcollection;
            return View("~/Views/Delivery/Web/DeliveryApproveList.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 수주승인 리스트 검색 AJAX
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ListApproveSearch(int page = 0)
        {
            //Idx Collection 신경써야한다.
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ListCheckSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ListSearch");
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
        ///  세부검색 - 항목을 클릭할 시에 나오는 Ajax
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/SubSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/SubSearch");
            LanguageType? lType = GetLanguageType();
            IList<OrderTableSub> list = orderTableSubDao.SelectOrderTableSub(idx, CompanySession.CompanyCode, lType);
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ApprovePage - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ApprovePage");
            String pLanguage = Request.Form["Lang"];
            LanguageType? lType = GetLanguageType();
            //정상경로가 아님
            if (idx < 1)
            {
                return ErrorPage("/Home/Error");
            }
            //발주서 검색
            OrderTable order = orderTableDao.SelectOrderTable(idx, CompanySession.CompanyCode);

            //정상경로가 아님
            if (order == null || order.Idx <= 0)
            {
                return ErrorPage("/Home/Error");
            }
            //문서정보 취득
            Document doc = documentDao.SelectDocument(order.Idx, DOCUMENTCODE, CompanySession.CompanyCode);

            //서버 상품 검색
            IList<OrderTableSub> subOrderList = orderTableSubDao.SelectOrderTableSub(idx, CompanySession.CompanyCode, lType);

            //기본ViewBag 설정
            ViewSetting(doc, order, subOrderList);

            //세션 저장
            Session["orderDOC"] = doc;
            Session["order"] = order;
            Session["orderSub"] = subOrderList;
            Session["orderCheck"] = ORDERSESSIONKEY;

            ViewBag.Commit = COMMIT_SHOUNINN;

            Session[Define.Session.ACTION] = "DeliveryApproveList";
            return View("~/Views/Delivery/Web/DeliveryApproveCheck.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 수주승인 폼에서 승인
        /// </summary>
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/Approve - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/Approve");
            //정상경로가 아님
            if (!String.IsNullOrEmpty(key) && !Object.Equals(Define.APPROVE, key) && !Object.Equals(Define.CANCEL, key))
            {
                return ErrorPage("/Home/Error");
            }
            Document pDoc = (Document)Session["orderDOC"];
            OrderTable pOrder = (OrderTable)Session["order"];
            IList<OrderTableSub> pSubOrder = (IList<OrderTableSub>)Session["orderSub"];
            String InputSessionkey = (String)Session["orderCheck"];

            Session["orderDOC"] = null;
            Session["order"] = null;
            Session["orderSub"] = null;
            Session["orderCheck"] = null;

            //비정상경로
            if (pDoc == null || pOrder == null || pSubOrder == null || InputSessionkey == null)
            {
                return Redirect("/Obtain/Order");
            }
            //비정상경로
            if (!ORDERSESSIONKEY.Equals(InputSessionkey))
            {
                return Redirect("/Obtain/Order");
            }

            //등록처리
            if (Object.Equals(Define.APPROVE, key))
            {
                orderTableDao.Approve(pOrder.Idx, App_Code.Define.STATE_APPLY, CompanySession.CompanyCode);

                //납품확인서 처리
                DeliveryTable delivery = new DeliveryTable();
                delivery.OrderCompany = pOrder.OrderName;
                delivery.OrderAddress = pOrder.OrderAddress;
                delivery.OrderSavedate = pOrder.OrderSaveDate;
                delivery.InorderCompany = CompanySession.CompanyName;
                delivery.InorderRepresentative = CompanySession.Representative;
                delivery.Creater = UserSession.UserName;
                delivery.CreateDate = DateTime.Now;
                delivery.State = App_Code.Define.STATE_NORMAL.ToString();
                delivery.CompanyCode = CompanySession.CompanyCode;
                deliveryTableDao.InsertDelivery(delivery);
                Int64 deliverylastIndex = deliveryTableDao.GetScopeIndentity();

                Document doc = new Document();
                doc.DocumentCode = documentDao.CreateCode();
                doc.DocumentType = DOCUMENTDELIVERYCODE;
                doc.DocumentIndex = delivery.Idx;
                doc.CreateDate = DateTime.Now;
                doc.Creater = UserSession.UserName;
                doc.State = App_Code.Define.STATE_NORMAL.ToString();
                doc.CompanyCode = CompanySession.CompanyCode;
                documentDao.InsertDocument(doc);

                foreach (OrderTableSub data in pSubOrder)
                {
                    //재고처리
                    ProductFlow product = new ProductFlow();
                    product.ProductIndex = data.ProductIndex;
                    product.ProductAmount = data.ProductAmount;
                    product.ProductSellPrice = data.ProductPrice;
                    product.State = Define.ProductFlow.OUTCOMESTANBY.ToString();
                    product.CompanyCode = CompanySession.CompanyCode;
                    product.ApplyType = pOrder.Idx;
                    product.Creater = UserSession.UserId;
                    product.CreteDate = DateTime.Now;
                    productFlowDao.InsertProductFlow(product);

                    //납품확인서(서브) 처리
                    DeliveryTableSub deliverySub = new DeliveryTableSub();
                    deliverySub.DeliveryKey = delivery.Idx;
                    deliverySub.Number = data.Number;
                    deliverySub.ProductIndex = data.ProductIndex;
                    deliverySub.ProductSpec = data.ProductSpec;
                    deliverySub.ProductType = data.ProductType;
                    deliverySub.ProductAmount = data.ProductAmount;
                    deliverySub.ProductPrice = data.ProductPrice;
                    deliverySub.ProductVat = 0;
                    deliverySub.Creater = UserSession.UserId;
                    deliverySub.CreateDate = DateTime.Now;
                    deliverySub.State = App_Code.Define.STATE_NORMAL.ToString();
                    deliverySub.CompanyCode = CompanySession.CompanyCode;
                    deliveryTableSubDao.InsertDeliveryTableSub(deliverySub);
                }
                //청구서 작성
                Bill bill = new Bill();
                bill.InorderCompany = CompanySession.CompanyName;
                bill.InorderRepresentative = CompanySession.Representative;
                bill.InorderPost = CompanySession.CompanyPostNumber;
                bill.InorderAddress = CompanySession.CompanyAddress;
                bill.OrderCompany = pOrder.OrderName;
                bill.OrderAddress = pOrder.OrderAddress;
                bill.BillDate = pOrder.PayDate;
                bill.BillMoney = pOrder.OrderMoney;
                bill.BillTax = 0;
                bill.BillTotal = bill.BillMoney + bill.BillTax;
                bill.Creater = UserSession.UserName;
                bill.CreateDate = DateTime.Now;
                bill.State = App_Code.Define.STATE_NORMAL.ToString();
                bill.CompanyCode = CompanySession.CompanyCode;
                billDao.InsertBill(bill);
                Int64 billLastIndex = billDao.GetScopeIndentity();

                Document doc2 = new Document();
                doc2.DocumentCode = documentDao.CreateCode();
                doc2.DocumentType = DOCUMENTBILLCODE;
                doc2.DocumentIndex = bill.Idx;
                doc2.CreateDate = DateTime.Now;
                doc2.Creater = UserSession.UserName;
                doc2.State = App_Code.Define.STATE_NORMAL.ToString();
                doc2.CompanyCode = CompanySession.CompanyCode;
                documentDao.InsertDocument(doc2);

                //기본ViewBag 설정
                ViewSetting(pDoc, pOrder, pSubOrder);
                ViewBag.Commit = COMMIT_SHOUNINN_COMPLATE;
                Session[Define.Session.ACTION] = "DeliveryApproveList";
                return View("~/Views/Delivery/Web/DeliveryApproveCheck.cshtml", Define.MASTER_VIEW);
            }
            else if (Object.Equals(Define.CANCEL, key))
            {
                //등록
                orderTableDao.Approve(pOrder.Idx, App_Code.Define.STATE_DELETE, CompanySession.CompanyCode);
                pSubOrder.AsParallel().ForAll((data) =>
                {
                    orderTableSubDao.ModifyState(data.Idx, App_Code.Define.STATE_DELETE, CompanySession.CompanyCode);
                });
                ViewBag.Commit = COMMIT_SHOUNINN_CANCLE;
                //기본ViewBag 설정
                ViewSetting(pDoc, pOrder, pSubOrder);
                Session[Define.Session.ACTION] = "DeliveryApproveList";
                return View("~/Views/Delivery/Web/DeliveryApproveCheck.cshtml", Define.MASTER_VIEW);
            }
            else
            {
                return ErrorPage("/Home/Error");
            }

        }
        /// <summary>
        /// 수주리스트(이력)
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult DeliveryOrderList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryOrderList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryOrderList");

            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, false) / (Double)PAGELIMIT)));
            IList<OrderTable> list = orderTableDao.SelectOrder(PAGELIMIT, Define.PAGE_START, ORDERCODE, CompanySession.CompanyCode, false);

            String idxcollection = "";
            LanguageType? lType = GetLanguageType();
            foreach (OrderTable pBuffer in list)
            {
                pBuffer.StateView(lType);
                idxcollection += pBuffer.Idx.ToString() + ",";
            }

            ViewBag.list = list;
            ViewBag.listcount = count;
            ViewBag.idxCollection = idxcollection;

            return View("~/Views/Delivery/Web/DeliveryOrderList.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 수주리스트 검색 AJAX
        /// </summary>
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
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ListSearch - NoAjax");
                return NoAjax();
            }
            int pagelimit = 50;
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Obtain/ListSearch");
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }

            int count = orderTableDao.CountOrder(ORDERCODE, CompanySession.CompanyCode, false);
            IList<OrderTable> list = orderTableDao.SelectOrder(pagelimit, page, ORDERCODE, CompanySession.CompanyCode, false);
            Dictionary<String, object> pRet = new Dictionary<String, object>();

            for (int i = 0; i < list.Count; i++)
            {
                pRet.Add("item" + i.ToString(), list[i]);
            }
            String idxcollection = "";
            LanguageType? lType = GetLanguageType();
            foreach (OrderTable pBuffer in list)
            {
                //상태
                pBuffer.StateView(lType);
                idxcollection += pBuffer.Idx.ToString() + ",";
            }
            pRet.Add("count", list.Count);
            pRet.Add("totalcount", count);
            pRet.Add("limit", pagelimit);
            pRet.Add("idxcollection", idxcollection);
            return Json(pRet, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 수주서 열람처리
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult DeliveryOrderView(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/OrderView - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/OrderView");
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
            IList<OrderTableSub> subOrder = orderTableSubDao.SelectOrderTableSub(idx, CompanySession.CompanyCode, lType);

            //기본ViewBag 설정
            ViewSetting(doc, order, subOrder);

            ViewBag.Commit = order.State;
            Session[Define.Session.ACTION] = "DeliveryOrderList";
            return View("~/Views/Delivery/Web/DeliveryOrderView.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 납품확인서
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult DeliveryCheckList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryCheckList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryCheckList");
            ViewBag.list = deliveryTableDao.SelectDelivery(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode);
            ViewBag.listcount = Convert.ToInt32(Math.Ceiling((Double)((Double)deliveryTableDao.GetDeliveryCount(CompanySession.CompanyCode) / (Double)PAGELIMIT)));
            return View("~/Views/Delivery/Web/DeliveryCheckList.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 납품확인서 검색
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult DeliveryCheckSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.AJAX_CHECK))
            {
                return Content("NG");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ListSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/CheckSearch");
            if (page < 1)
            {
                page = 1;
            }
            int count = deliveryTableDao.GetDeliveryCount(CompanySession.CompanyCode);
            IList<DeliveryTable> list = deliveryTableDao.SelectDelivery(PAGELIMIT, page, CompanySession.CompanyCode);
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
        /// 납품확인서 View
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult DeliveryView(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }

            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryCheckList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryView");
            LanguageType? lType = GetLanguageType();

            DeliveryTable delivery = deliveryTableDao.SelectByIdx(idx, CompanySession.CompanyCode);
            IList<DeliveryTableSub> sub = deliveryTableSubDao.SelectSubList(idx, CompanySession.CompanyCode, lType);
            Document doc = documentDao.SelectDocument(idx, DOCUMENTDELIVERYCODE, CompanySession.CompanyCode);
            Decimal total = 0;
            Decimal taxtotal = 0;
            foreach (DeliveryTableSub pData in sub)
            {
                total += pData.ProductPrice;
                taxtotal += pData.ProductVat;
            }
            ViewBag.delivery = delivery;
            ViewBag.deliverySub = sub;
            ViewBag.document = doc;
            ViewBag.total = total;
            ViewBag.taxtotal = taxtotal;

            Session[Define.Session.ACTION] = "DeliveryCheckList";
            return View("~/Views/Delivery/Web/DeliveryView.cshtml", Define.MASTER_VIEW);

        }
        /// <summary>
        /// 청구서 리스트
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult DeliveryBillList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryBillList - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/DeliveryBillList");
            ViewBag.list = billDao.SelectBill(PAGELIMIT, Define.PAGE_START, CompanySession.CompanyCode);
            ViewBag.listcount = Convert.ToInt32(Math.Ceiling((Double)((Double)billDao.GetBillCountByCompanyCode(CompanySession.CompanyCode) / (Double)PAGELIMIT)));
            return View("~/Views/Delivery/Web/DeliveryBillList.cshtml", Define.MASTER_VIEW);
        }
        /// <summary>
        /// 청구서 검색
        /// </summary>
        [AjaxFilterAttribute]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult BillSearch(int page = 0)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/ListSearch - NoAjax");
                return NoAjax();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/BillSearch");
            if (page < Define.PAGE_START)
            {
                page = Define.PAGE_START;
            }
            int count = billDao.GetBillCountByCompanyCode(CompanySession.CompanyCode);
            IList<Bill> list = billDao.SelectBill(PAGELIMIT, page, CompanySession.CompanyCode);
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
        ///청구서 View
        /// </summary>
        /// <returns></returns>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult BillView(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/BillView - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Delivery/BillView");

            ViewBag.bill = billDao.SelectByIdx(idx, CompanySession.CompanyCode);
            Session[Define.Session.ACTION] = "DeliveryBillList";
            return View("~/Views/Delivery/Web/BillView.cshtml", Define.MASTER_VIEW);
        }
    }
}
