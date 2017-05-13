using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace Household.Common
{
    public class HttpConnector
    {
        private static String serviceUrl = "";
        private static String dataUrl = "";
        public enum HttpMethod
        {
            POST,
            GET
        }
        public static string GetRequest(String url, HttpMethod method, IDictionary<String, string> param = null)
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
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private static string combineParameter(IDictionary<String, string> param)
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

        public static string GetDataRequest(String code, IDictionary<String, string> param = null)
        {
            String connUrl = serviceUrl + code;
            String key = GetRequest(connUrl, HttpMethod.POST, param);
            return GetRequest(dataUrl,
                              HttpMethod.POST,
                              new Dictionary<String, String>() { 
                              { "CODE", key } });
        }
    }
}