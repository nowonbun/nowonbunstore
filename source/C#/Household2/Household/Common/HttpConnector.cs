using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Household.Common
{
    public class HttpConnector
    {
        private String serviceUrl = null;
        private static HttpConnector instance = null;
        public static HttpConnector CreateInstance(String serviceUrl)
        {
            instance = new HttpConnector(serviceUrl);
            return instance;
        }
        public static HttpConnector GetInstance()
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }
            return instance;
        }
        public enum HttpMethod
        {
            POST,
            GET
        }
        public HttpConnector(String serviceUrl)
        {
            this.serviceUrl = serviceUrl;
        }

        private string GetRequest(String url, HttpMethod method, String param = null)
        {
            try
            {
                if (HttpMethod.GET.Equals(method) && param != null)
                {
                    url += (url.IndexOf("?") != -1) ? "&" : "?" + param;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                if (HttpMethod.POST.Equals(method) && param != null)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);
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
                throw e;
                //return GetRequest(url, method, param);
            }
        }
        public string GetRequest(String url, HttpMethod method, IDictionary<String, Object> param = null)
        {
            try
            {
                string paramStr = param != null ? combineParameter(param) : null;
                if (HttpMethod.GET.Equals(method) && paramStr != null)
                {
                    url += (url.IndexOf("?") != -1) ? "&" : "?" + paramStr;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                request.ContentType = "application/x-www-form-urlencoded";
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
                throw e;
                //return GetRequest(url, method, param);
            }
        }

        private string combineParameter(IDictionary<string, Object> param)
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

        public string GetDataRequest(String code, IDictionary<String, Object> param)
        {
            String paramBuffer = "p=sY";
            if (param != null)
            {
                String json = JsonConvert.SerializeObject(param);
                String base64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                paramBuffer += base64;
            }
            String connUrl = serviceUrl + code;
            String ret = GetRequest(connUrl, HttpMethod.POST, paramBuffer);
            if(ret.Length < 2)
            {
                return null;
            }
            ret = ret.Substring(2);
            byte[] data = System.Convert.FromBase64String(ret);
            return Encoding.UTF8.GetString(data);
        }
    }
}