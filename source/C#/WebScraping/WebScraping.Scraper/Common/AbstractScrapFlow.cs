using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Node;
using Gecko;
using Gecko.DOM;
using WebScraping.Library.Log;
using WebScraping.Scraper.Impl;
using WebScraping.Dao.Entity;
using WebScraping.Dao.Common;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Attribute;
using System.Net;
using System.IO;

namespace WebScraping.Scraper.Common
{
    abstract class AbstractScrapFlow : Allocation, IScrapFlow
    {
        protected ScrapParameter Parameter { get; private set; }
        protected String StartPageUrl { get; set; }
        protected Dictionary<String, Func<GeckoDocument, Uri, Boolean>> FlowMap = new Dictionary<string, Func<GeckoDocument, Uri, Boolean>>();
        private ScrapBrowser browser;
        private IList<ScrapingCommonData> commondata = new List<ScrapingCommonData>();
        private IList<ScrapingPackageData> packagedata = new List<ScrapingPackageData>();

        [ResourceDao]
        private IScrapingCommonDataDao commondao;
        [ResourceDao]
        private IScrapingPakageDataDao packagedao;

        protected Logger logger { get; private set; }

        public enum HttpMethod
        {
            POST,
            GET
        }

        public AbstractScrapFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
        {
            Parameter = param;
            this.browser = browser;
            this.commondao = FactoryDao.GetInstance().GetDao("WebScraping.Dao.Dao.Impl.ScrapingCommonDataDao") as IScrapingCommonDataDao;
            this.packagedao = FactoryDao.GetInstance().GetDao("WebScraping.Dao.Dao.Impl.ScrapingPakageDataDao") as IScrapingPakageDataDao;

            logger = LoggerBuilder.Init().Set(this.GetType());
        }
        protected virtual Boolean NotAction(GeckoDocument document, Uri uri)
        {
            logger.Info("NotAction uri : " + uri);
            return true;
        }
        public String StartPage()
        {
            return StartPageUrl;
        }
        public Func<GeckoDocument, Uri, Boolean> Procedure(Uri uri)
        {
            logger.Info("Procedure uri : " + uri);
            String key = "";
            IList<String> temp = FlowMap.Keys.Where(k => { return uri.ToString().IndexOf(k) != -1; }).ToList();
            if (temp.Count < 1)
            {
                return NotAction;
            }
            else if (temp.Count > 1)
            {
                int pos = uri.ToString().IndexOf("?");
                String urlTemp;
                if (pos != -1)
                {
                    urlTemp = uri.ToString().Substring(0, pos);
                }
                else
                {
                    urlTemp = uri.ToString();
                }
                key = temp.Where(k =>
                {
                    int index = urlTemp.LastIndexOf(k);
                    int length = urlTemp.Length;
                    int ret = length - index - k.Length;
                    return ret < 1;
                }).SingleOrDefault();
            }
            else
            {
                key = temp[0];
            }

            return FlowMap[key];
        }
        protected void Navigate(String url)
        {
            this.browser.Navigate(url);
        }
        protected void SetCommonData(int index, String data)
        {
            ScrapingCommonData node = new ScrapingCommonData();
            node.KeyCode = this.Parameter.Keycode;
            node.keyIndex = index;
            node.Data = data;
            node.CreateDate = DateTime.Now;
            commondata.Add(node);
        }
        protected void SetPackageData(int index, int separation, String data)
        {
            ScrapingPackageData node = new ScrapingPackageData();
            node.KeyCode = this.Parameter.Keycode;
            node.keyIndex = index;
            node.Separation = separation;
            node.Data = data;
            node.CreateDate = DateTime.Now;
            packagedata.Add(node);
        }
        protected void UpdateData()
        {
            foreach (ScrapingCommonData node in commondata)
            {
                commondao.Insert(node);
            }
            foreach (ScrapingPackageData node in packagedata)
            {
                packagedao.Insert(node);
            }
        }
        public void End()
        {
            UpdateData();
            Finally();
        }
        public string GetRequest(String url, GeckoDocument document, HttpMethod method, IDictionary<String, Object> param = null)
        {
            try
            {
                string paramStr = param != null ? CombineParameter(param) : null;
                if (HttpMethod.GET.Equals(method) && paramStr != null)
                {
                    url += (url.IndexOf("?") != -1) ? "&" : "?" + paramStr;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                request.ContentType = "application/x-www-form-urlencoded";

                //cookie set??
                String cookie = browser.Document.Cookie;
                if (cookie != null && !String.IsNullOrEmpty(cookie))
                {
                    String[] buffer = cookie.Split(';');
                    request.CookieContainer = new CookieContainer();
                    foreach (var b in buffer)
                    {
                        String temp = b.Trim();
                        int pos = temp.IndexOf("=");
                        System.Net.Cookie c = new System.Net.Cookie();
                        c.Domain = browser.Document.Domain;
                        c.Name = temp.Substring(0, pos);
                        c.Value = temp.Substring(pos + 1);
                        request.CookieContainer.Add(c);
                    }
                }
                if (HttpMethod.POST.Equals(method) && paramStr != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(paramStr);
                    request.ContentLength = byteArray.Length;
                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e.ToString());
                throw e;
            }
        }
        private string CombineParameter(IDictionary<string, Object> param)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String key in param.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(key).Append("=").Append(param[key]);
            }
            return sb.ToString();
        }
        protected void PostAjaxJson(GeckoDocument document, string ajaxurl, IDictionary<String, Object> param)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("<HTML>");
            buffer.Append("<BODY>");
            buffer.Append("<FORM ACTION='").Append(ajaxurl).Append("' method='POST'>");
            foreach (String key in param.Keys)
            {
                buffer.Append("<INPUT type='hidden' name='")
                    .Append(key)
                    .Append("' id='")
                    .Append(key)
                    .Append("' value='")
                    .Append(param[key])
                    .Append("'>");
            }
            buffer.Append("<INPUT type='submit' id='trigger'>");
            buffer.Append("</FORM>");
            buffer.Append("</BODY>");
            buffer.Append("</HTML>");
            document.Body.InnerHtml = buffer.ToString();
            document.GetElementById<GeckoInputElement>("trigger").Click();
        }
        protected abstract void Finally();
    }
}
