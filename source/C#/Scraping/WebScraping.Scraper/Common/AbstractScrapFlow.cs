using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebScraping.Scraper.Interface;
using Gecko;
using Gecko.DOM;
using WebScraping.Library.Log;
using WebScraping.Scraper.Impl;
using System.Net;
using System.IO;
using WebScraping.Library.Config;
using System.Reflection;
using System.Threading;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Common
{
    abstract class AbstractScrapFlow : IScrapFlow
    {
        //private Dictionary<String, Func<GeckoDocument, Uri, FlowModelData, FlowModelData>> flowMap = new Dictionary<string, Func<GeckoDocument, Uri, FlowModelData, FlowModelData>>();
        private FlowList flowlist = new FlowList();
        private Dictionary<Type, IList<FieldInfo>> reflectFlyweight = new Dictionary<Type, IList<FieldInfo>>();
        private List<Type> flyweightKeys = new List<Type>();
        private ScrapBrowser browser;
        private StringBuilder buffer = new StringBuilder();
        private Uri currectUri;

        protected Parameter Parameter { get; private set; }
        protected Response Response { get; set; }
        protected FlowList FlowList
        {
            //Next Implement
            get { return flowlist; }
        }
        protected Dictionary<Type, IList<FieldInfo>> ReflectFlyweight
        {
            get { return reflectFlyweight; }
        }
        protected List<Type> ReflectFlyweightKeys
        {
            get { return flyweightKeys; }
        }
        protected Uri CurrectUri
        {
            get { return currectUri; }
        }
        protected Logger logger { get; private set; }
        public enum HttpMethod
        {
            POST,
            GET
        }

        public AbstractScrapFlow(ScrapBrowser browser, Parameter param)
        {
            Parameter = param;
            this.browser = browser;
            Response = new Response(Parameter.Key);
            //TODO: I don't like this type, will want to change the type of datetime. A:데이터 전송시 string으로 어차피 변환해야 하지 않나요??
            Response.Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            logger = LoggerBuilder.Init().Set(this.GetType());
            this.browser.ProgressChanged += (s, e) =>
            {
                //logger.Debug("CurrentProgress/MaximumProgress : " + e.CurrentProgress + "/" + e.MaximumProgress);
            };
        }
        public void Initialize(FlowModelData flowModelData)
        {
            switch (Parameter.ScrapType)
            {
                case ScrapType.MemberInfo:
                    ScrapType0(flowModelData);
                    break;
                case ScrapType.SalesInfo:
                    ScrapType1(flowModelData);
                    break;
                case ScrapType.CalculationInfo:
                    ScrapType2(flowModelData);
                    break;
                case ScrapType.SettledInfo1:
                    ScrapType3(flowModelData);
                    break;
                case ScrapType.SettledInfo2:
                    ScrapType4(flowModelData);
                    break;
                case ScrapType.ReturnInfo:
                    ScrapType5(flowModelData);
                    break;
            }
        }
        protected virtual void NotAction(FlowModelData model)
        {
            logger.Info("NotAction uri : " + model.Uri);
        }
        protected virtual void NotAction(String url, String filename)
        {
            logger.Info("NotAction uri : " + filename);
        }
        public void Procedure(Uri uri, out Action<FlowModelData> flow, out String next, out Action<FlowModelData> callback)
        {
            logger.Info("Procedure uri : " + uri);
            currectUri = uri;
            String key = "";
            IList<String> temp = flowlist.UrlList.Where(k => { return uri.ToString().IndexOf(k) != -1; }).ToList();
            if (temp.Count < 1)
            {
                flow = NotAction;
                next = null;
                callback = null;
                return;
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

            var f = flowlist[key];
            flow = f.Func;
            next = f.Next;
            callback = f.Callback;
        }
        protected void Navigate(String url)
        {
            this.browser.Navigate(url);
        }
        /*
         * 공통데이터 전송 ( 회원정보 data type -> string ) 
         */
        protected void SetCommonData(int index, String data)
        {
            Result node = new Result(Parameter.Key)
            {
                Index = index,
                Resulttype = ResultType.Common,
                Data = data
            };
            BrokerSender.Instance.Send(node.ToJson());
        }
        /*
         * 공통데이터외 전송 ( 매출내역,정산내역,정산예정금,반품율 등 data type -> json string ) 
         * 4000byte로 단위로 Separation으로 구분하여 전송함
         * TODO:json구조는 {"value":[{data01~data30},{data01~data30}]}...?
         */
        protected void SetPackageData(int index, int separation, String data)
        {
            Result node = new Result(Parameter.Key)
            {
                Resulttype = ResultType.Pacakage,
                Separation = separation,
                Index = index,
                Data = data
            };
            BrokerSender.Instance.Send(node.ToJson());
        }
        protected void Exit(FlowModelData flowModelData)
        {
            End(flowModelData);
            Scraper.Exit();
        }
        /*
         * 정상종료 이벤트
         */
        public void End(FlowModelData flowModelData)
        {
            Finally(flowModelData);
            Response.Resulttype = ResultType.Exit;
            Response.Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            BrokerSender.Instance.Send(Response.ToJson());
        }
        /*
         * 에러종료 이벤트
         * TODO:Error code....?
         */
        public void Error(Exception e)
        {
            Response.Resulttype = ResultType.Error;
            Response.Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            BrokerSender.Instance.Send(Response.ToJson());
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

        protected Dictionary<String, String> Param(Uri uri)
        {
            Dictionary<String, String> ret = new Dictionary<string, string>();
            String temp = uri.ToString();
            int pos = uri.ToString().IndexOf("?");
            if (pos < 0)
            {
                return ret;
            }
            temp = temp.Substring(pos + 1, temp.Length - (pos + 1));
            String[] buffer = temp.Split('&');
            foreach (var b in buffer)
            {
                pos = b.IndexOf("=");
                if (pos < 0)
                {
                    continue;
                }
                var k = b.Substring(0, pos);
                var v = b.Substring(pos + 1, b.Length - (pos + 1));
                if (ret.ContainsKey(k))
                {
                    ret.Remove(k);
                }
                ret.Add(k, v);
            }
            return ret;
        }
        protected void SetTimeout(Action func, int delay = 1000)
        {
            ThreadPool.QueueUserWorkItem(c =>
            {
                Thread.Sleep(delay);
                func();
            });
        }
        public abstract String StartPage(FlowModelData flowModelData);
        protected abstract void Finally(FlowModelData flowModelData);
        protected abstract void ScrapType0(FlowModelData flowModelData);
        protected abstract void ScrapType1(FlowModelData flowModelData);
        protected abstract void ScrapType2(FlowModelData flowModelData);
        protected abstract void ScrapType3(FlowModelData flowModelData);
        protected abstract void ScrapType4(FlowModelData flowModelData);
        protected abstract void ScrapType5(FlowModelData flowModelData);
    }
}
