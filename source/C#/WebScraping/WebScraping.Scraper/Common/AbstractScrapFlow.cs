using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WebScraping.Library.Config;
using System.Reflection;
using System.Threading;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Common
{
    /*
        pingpong & end message!
        window handler~
    */
    abstract class AbstractScrapFlow : Allocation, IScrapFlow
    {
        protected ScrapParameter Parameter { get; private set; }
        //protected String StartPageUrl { get; set; }
        protected Dictionary<String, Func<GeckoDocument, Uri, Boolean>> FlowMap
        {
            get { return flowMap; }
        }
        protected Dictionary<String, Action<String, String>> DownloadMap
        {
            get { return downloadMap; }
        }
        protected Dictionary<Type, IList<FieldInfo>> ReflectFlyweight
        {
            get { return reflectFlyweight; }
        }
        protected List<Type> ReflectFlyweightKeys
        {
            get { return flyweightKeys; }
        }
        private Dictionary<String, Func<GeckoDocument, Uri, Boolean>> flowMap = new Dictionary<string, Func<GeckoDocument, Uri, Boolean>>();
        private Dictionary<String, Action<String, String>> downloadMap = new Dictionary<String, Action<String, String>>();
        private Dictionary<Type, IList<FieldInfo>> reflectFlyweight = new Dictionary<Type, IList<FieldInfo>>();
        private List<Type> flyweightKeys = new List<Type>();
        private ScrapBrowser browser;
        private IList<ScrapingCommonData> common_data_list = new List<ScrapingCommonData>();
        private IList<ScrapingPackageData> package_data_list = new List<ScrapingPackageData>();
        private StringBuilder buffer = new StringBuilder();
        [ResourceDao]
        private IScrapingCommonDataDao commondao;
        [ResourceDao]
        private IScrapingPackageDataDao packagedao;

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
            this.commondao = FactoryDao.GetInstance().GetDao<IScrapingCommonDataDao>();
            this.packagedao = FactoryDao.GetInstance().GetDao<IScrapingPackageDataDao>();

            logger = LoggerBuilder.Init().Set(this.GetType());
            //this.browser.InitializeDownLoad(ExcelDownload);
            this.browser.ProgressChanged += (s, e) =>
            {
                //logger.Debug("CurrentProgress/MaximumProgress : " + e.CurrentProgress + "/" + e.MaximumProgress);
            };
        }
        protected virtual Boolean NotAction(GeckoDocument document, Uri uri)
        {
            logger.Info("NotAction uri : " + uri);
            return true;
        }
        protected virtual void NotAction(String url, String filename)
        {
            logger.Info("NotAction uri : " + filename);
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
        public Action<String, String> DownloadProcedure(String url)
        {
            logger.Info("Download uri : " + url);
            String key = "";
            IList<String> temp = DownloadMap.Keys.Where(k => { return url.IndexOf(k) != -1; }).ToList();
            if (temp.Count < 1)
            {
                return NotAction;
            }
            else
            {
                key = temp[0];
            }

            return DownloadMap[key];
        }
        protected void Navigate(String url)
        {
            this.browser.Navigate(url);
        }
        protected void SetCommonData(int index, String data)
        {
            ScrapingCommonData node = new ScrapingCommonData();
            node.KeyCode = this.Parameter.Keycode;
            node.KeyIndex = index;
            node.Data = data;
            node.CreateDate = DateTime.Now;
            common_data_list.Add(node);
        }
        protected void SetPackageData(int index, int separation, String data)
        {
            ScrapingPackageData node = new ScrapingPackageData();
            node.KeyCode = this.Parameter.Keycode;
            node.KeyIndex = index;
            node.Separation = separation;
            node.Data = data;
            node.CreateDate = DateTime.Now;
            package_data_list.Add(node);
        }
        protected void UpdateData()
        {
            if (Debug.IsDebug())
            {
                try
                {
                    foreach (var item in common_data_list)
                    {
                        this.buffer.Append(item.KeyCode).Append("||");
                        this.buffer.Append(item.KeyIndex).Append("||");
                        this.buffer.Append(item.Data).Append("||");
                        this.buffer.Append(item.CreateDate).AppendLine();
                    }
                    String filepath = Path.Combine(ConfigSystem.ReadConfig("Config", "Temp", "Path"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv");
                    using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(this.buffer.ToString());
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    logger.Debug("common_data_list - " + filepath);
                }
                finally
                {
                    this.buffer.Clear();
                }
                try
                {
                    foreach (var item in package_data_list)
                    {
                        this.buffer.Append(item.KeyCode).Append("||");
                        this.buffer.Append(item.KeyIndex).Append("||");
                        this.buffer.Append(item.Separation).Append("||");
                        this.buffer.Append(item.Data).Append("||");
                        this.buffer.Append(item.CreateDate).AppendLine();
                    }
                    String filepath = Path.Combine(ConfigSystem.ReadConfig("Config", "Temp", "Path"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv");
                    using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(this.buffer.ToString());
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    logger.Debug("package_data_list - " + filepath);
                }
                finally
                {
                    this.buffer.Clear();
                }
                Finally();
            }
            else
            {
                commondao.InsertList(common_data_list);
                packagedao.InsertList(package_data_list);
            }
        }
        public void End()
        {
            UpdateData();
            Finally();
            this.browser.NotifyEnd(Parameter.Keycode);
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
            try
            {
                foreach (String key in param.Keys)
                {
                    if (this.buffer.Length > 0)
                    {
                        this.buffer.Append("&");
                    }
                    this.buffer.Append(key).Append("=").Append(param[key]);
                }
                return this.buffer.ToString();
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        protected void PostAjaxJson(GeckoDocument document, string ajaxurl, IDictionary<String, Object> param)
        {
            try
            {
                this.buffer.Append("<HTML>");
                this.buffer.Append("<BODY>");
                this.buffer.Append("<FORM ACTION='").Append(ajaxurl).Append("' method='POST'>");
                foreach (String key in param.Keys)
                {
                    this.buffer.Append("<INPUT type='hidden' name='")
                        .Append(key)
                        .Append("' id='")
                        .Append(key)
                        .Append("' value='")
                        .Append(param[key])
                        .Append("'>");
                }
                this.buffer.Append("<INPUT type='submit' id='trigger'>");
                this.buffer.Append("</FORM>");
                this.buffer.Append("</BODY>");
                this.buffer.Append("</HTML>");
                if (browser.Document != document)
                {
                    logger.Debug("Browser!!!!!!!!!");
                    document = browser.Document;
                }
                document.Body.InnerHtml = this.buffer.ToString();
                document.GetElementById<GeckoInputElement>("trigger").Click();

            }
            finally
            {
                this.buffer.Clear();
            }
        }
        protected void ExcuteJavascript(GeckoDocument document, String script)
        {
            GeckoElement scriptelement = document.CreateElement("script");
            scriptelement.SetAttribute("type", "text/javascript");
            scriptelement.TextContent = script;
            document.Head.AppendChild(scriptelement);
        }
        protected String CreateGetParameter(IDictionary<String, String> param)
        {
            try
            {
                foreach (var node in param)
                {
                    this.buffer.Append(node.Key);
                    this.buffer.Append("=");
                    this.buffer.Append(node.Value);
                    this.buffer.Append("&");
                }
                this.buffer.Remove(this.buffer.Length - 1, 1);
                return this.buffer.ToString();
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        protected String ToJson(IList<FieldInfo> fields, Object data)
        {
            try
            {
                this.buffer.Append("{");
                foreach (var field in fields)
                {
                    this.buffer.Append("\"");
                    this.buffer.Append(field.Name);
                    this.buffer.Append("\":\"");
                    this.buffer.Append(field.GetValue(data));
                    this.buffer.Append("\",");
                }
                this.buffer.Remove(buffer.Length - 1, 1);
                this.buffer.Append("}");
                return this.buffer.ToString();
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        protected String ToExcelJson(IList<FieldInfo> fields, Object data)
        {
            try
            {
                var sortedFields = fields
                    .Where(field =>
                    {
                        return field.GetCustomAttribute(typeof(ExcelHeader)) as ExcelHeader != null;
                    })
                    .OrderBy(field =>
                    {
                        ExcelHeader header = field.GetCustomAttribute(typeof(ExcelHeader)) as ExcelHeader;
                        return header.ColumnIndex;
                    });
                this.buffer.Append("{");
                foreach (var field in sortedFields)
                {
                    ExcelHeader header = field.GetCustomAttribute(typeof(ExcelHeader)) as ExcelHeader;
                    this.buffer.Append("\"");
                    this.buffer.Append(header.HeaderName);
                    this.buffer.Append("\":\"");
                    this.buffer.Append(field.GetValue(data));
                    this.buffer.Append("\",");
                }
                this.buffer.Remove(buffer.Length - 1, 1);
                this.buffer.Append("}");
                return this.buffer.ToString();
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        protected void WaitFile(String file, Action action)
        {
            ThreadPool.QueueUserWorkItem(c =>
            {
                while (true)
                {
                    if (File.Exists(file))
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
                action();
            });
        }

        public abstract String StartPage();
        protected abstract void Finally();
    }
}
