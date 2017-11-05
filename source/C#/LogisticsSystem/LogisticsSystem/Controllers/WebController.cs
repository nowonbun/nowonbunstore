using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LogisticsSystem.Models;
using LogisticsSystem.App_Code;
using LogisticsSystem.Dao;
using System.Web.Security;
using LogisticsSystem.Validate;

namespace LogisticsSystem.Controllers
{
    public class WebController : AbstractController
    {
        private const String MAIN = "Main";
        private const int PAGELIMIT = 50;
        private UserInfoDao userInfoDao = FactoryDao.Instance().GetUserInfoDao();
        private CompanyInfoDao companyInfoDao = FactoryDao.Instance().GetCompanyInfoDao();
        private ConnectDao connectDao = FactoryDao.Instance().GetConnectDao();

        [PipelineFilter]
        public ActionResult Index()
        {
            ViewBag.ErrorMsg = Session[Define.Session.ERROR_MESSAGE];
            ViewBag.IdBuffer = Session[Define.Session.ID];
            Session[Define.Session.ERROR_MESSAGE] = "";
            Session[Define.Session.ID] = "";
            return View();
        }

        public ActionResult Login(UserInfo pInfo)
        {
            Session[Define.Session.ID] = pInfo.UserId;
            UserInfo userinfo = userInfoDao.SelectUserInfo(pInfo.UserId, pInfo.Password);
            if (userinfo == null)
            {
                LanguageType? lType = (LanguageType?)Session[Define.Session.LANGUAGE_TYPE];
                if (Object.Equals(lType, LanguageType.Korea))
                {
                    Session[Define.Session.ERROR_MESSAGE] = "아이디 또는 패스워드를 확인해 주십시오.";
                }
                else
                {
                    Session[Define.Session.ERROR_MESSAGE] = "ユーザIDまたはパスワードを確認してください。";
                }
                return Redirect(FormsAuthentication.LoginUrl);
            }
            FormsAuthentication.SetAuthCookie(userinfo.UserId, false);
            Session[Define.Session.USER_INFO] = userinfo;

            CompanyInfo compnayInfo = companyInfoDao.SelectCompanyInfo(userinfo.CompanyCode);
            compnayInfo.NumberSplit();
            Session[Define.Session.COMPANY_INFO] = compnayInfo;

            Connect conn = new Connect();
            conn.UserId = userinfo.UserId;
            conn.ConnectDate = DateTime.Now;
            conn.IpAddress = Request.UserHostAddress;
            connectDao.InsertConnect(conn);

            LogWriter.Instance().LogWrite(pInfo.UserId, "Login Start");
            return Redirect(MAIN);

        }

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
                return base.Logout();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/Main");
            return View(MAIN, Define.MASTER_VIEW);
        }

        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult CompanyInfo()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                return base.Logout();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/CompanyInfo");
            ViewBag.companyInfo = CompanySession;
            return View("~/Views/Web/CompanyInfo.cshtml", Define.MASTER_VIEW);
        }

        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult CompanyInsert(CompanyInfo rComp)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/CompanyInsert");

            rComp.Creater = UserSession.UserId;
            rComp.CreateDate = DateTime.Now;

            LanguageType? lType = (LanguageType?)Session[Define.Session.LANGUAGE_TYPE];

            IList<String> pError = rComp.Validate(lType);
            if (pError.Count > 0)
            {
                String ErrMsg = "";
                foreach (String pBuffer in pError)
                {
                    ErrMsg += pBuffer + "<br>";
                }
                ViewBag.ErrMsg = ErrMsg;
                ViewBag.companyInfo = rComp;
                Session["action"] = "CompanyInfo";
                return View("~/Views/Web/CompanyInfo.cshtml", Define.MASTER_VIEW);
            }

            String message = "";
            companyInfoDao.Modify(rComp, lType);
            if (Object.Equals(lType, LanguageType.Korea))
            {
                message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 수정되었습니다.";
            }
            else
            {
                message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 修正しました。";
            }
            rComp = CompanySession;
            Session[Define.Session.COMPANY_INFO] = rComp;

            ViewBag.ErrMsg = message;
            ViewBag.companyInfo = rComp;
            Session[Define.Session.ACTION] = "CompanyInfo";
            return View("~/Views/Web/CompanyInfo.cshtml", Define.MASTER_VIEW);
        }

        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult UserInfo()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                return base.Logout();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/UserInfo");
            ViewBag.UserInfo = UserSession;
            return View("~/Views/Web/UserInfo.cshtml", Define.MASTER_VIEW);
        }

        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult UserInsert(UserInfo rUser)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                return base.Logout();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/UserInsert");
            rUser.CreateDate = DateTime.Now;
            rUser.Creater = UserSession.UserId;
            
            LanguageType? lType = (LanguageType?)Session[Define.Session.LANGUAGE_TYPE];
            IList<String> pError = rUser.Validate(lType);

            if (pError.Count > 0)
            {
                String ErrMsg = "";
                foreach (String pBuffer in pError)
                {
                    ErrMsg += pBuffer + "<br>";
                }
                ViewBag.ErrMsg = ErrMsg;
                ViewBag.UserInfo = rUser;
                Session[Define.Session.ACTION] = "UserInfo";

                return View("~/Views/Web/UserInfo.cshtml", Define.MASTER_VIEW);
            }

            String message = "";

            if (Object.Equals(lType, LanguageType.Korea))
            {
                message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 수정되었습니다.";
            }
            else
            {
                message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 修正しました。";
            }
            rUser.Idx = UserSession.Idx;
            rUser.Password = UserSession.Password;
            rUser.Permission = UserSession.Permission;
            rUser.CompanyCode = CompanySession.CompanyCode;
            rUser.State = Define.NOTDEL;
            Session[Define.Session.USER_INFO] = rUser;
            userInfoDao.ModifyUserInfo(rUser, lType,CompanySession.CompanyCode);
            ViewBag.ErrMsg = message;
            ViewBag.UserInfo = rUser;

            Session[Define.Session.ACTION] = "UserInfo";
            return View("~/Views/Web/UserInfo.cshtml", Define.MASTER_VIEW);
        }

        /// <summary>
        /// 언어선택화면
        /// </summary>
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult LanguageSelectView()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/LanguageSelectView - Error");
                return base.Logout();
            }
            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/LanguageSelectView");

            object returnController = Session["ReturnController"];
            object returnAction = Session["ReturnAction"];
            object returnUrl = Session["ReturnURL"];
            object printUrl = Session["PrintURL"];

            Session["ReturnController"] = null;
            Session["ReturnAction"] = null;
            Session["ReturnURL"] = null;
            Session["PrintURL"] = null;

            //정상경로가 아님
            if (returnAction == null || returnAction == null || returnUrl == null || printUrl == null)
            {
                return ErrorPage("/Home/Error");
            }

            Session[Define.Session.CONTROLLER] = returnController;
            Session[Define.Session.ACTION] = returnAction;

            ViewBag.returnUrl = returnUrl;
            ViewBag.printUrl = printUrl;

            return View("~/Views/Web/LanguageSelectView.cshtml", Define.MASTER_VIEW);

        }

        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult ConnectList()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/ConnectList 인증에러");
                return base.Logout();
            }

            LogWriter.Instance().LogWrite(UserSession.UserId, "/Web/ConnectList");
            ViewBag.list = connectDao.SelectConnect(PAGELIMIT, Define.PAGE_START);
            ViewBag.listcount = Convert.ToInt32(Math.Ceiling((Double)((Double)connectDao.GetConnectCount() / (Double)PAGELIMIT)));

            return View("~/Views/Web/ConnectLog.cshtml", Define.MASTER_VIEW);
        }
    }
}
