using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;
using System.Web.Mvc;
using Household.Models.Bean;

namespace Household.Common
{
    public interface IFlow
    {
        bool Validate();
        ActionResult Run();
        ActionResult Error();
    }

    public abstract class AbstractFlow : IFlow
    {
        private ILog logger;

        private HttpRequestBase request;
        private HttpResponseBase response;
        private HttpContextBase context;
        private AjaxResultBean bean;
        private ActionResult result;

        protected AbstractFlow(HttpRequestBase request, HttpResponseBase response, HttpContextBase context)
        {
            this.request = request;
            this.response = response;
            this.context = context;

            logger = LogManager.GetLogger(this.GetType());
            bean = new AjaxResultBean();

        }

        public static implicit operator ActionResult(AbstractFlow flow)
        {
            return flow.result;
        }

        protected HttpRequestBase Request
        {
            get { return this.request; }
        }

        protected HttpResponseBase Response
        {
            get { return this.response; }
        }

        protected HttpContextBase Context
        {
            get { return this.context; }
        }

        protected AjaxResultBean ResultBean
        {
            get { return this.bean; }
        }

        protected ILog Logger
        {
            get { return logger; }
        }

        public LoginBean UserSession
        {
            get
            {
                return context.Session[Define.USER_SESSION_NAME] as LoginBean;
            }
            set
            {
                context.Session[Define.USER_SESSION_NAME] = value;
            }
        }
        public void SetCookie(String name, String value)
        {
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(cookie);
        }
        public String GetCookie(String name)
        {
            return Request.Cookies[name] != null ? Request.Cookies[name].Value : null;
        }

        protected JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            JsonResult ret = new JsonResult();
            ret.Data = data;
            ret.JsonRequestBehavior = behavior;
            return ret;
        }

        public AbstractFlow Execute()
        {
            if (Validate())
            {
                result = Run();
            }
            else
            {
                result = Error();
            }
            return this;
        }

        public abstract bool Validate();
        public abstract ActionResult Run();
        public abstract ActionResult Error();
    }
}