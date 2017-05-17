using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace DataChecker
{
    class Program
    {

        public enum HttpMethod
        {
            POST,
            GET
        }
        static void Main(string[] args)
        {
            //1493046000000
            //1490414594000
            /*Int64 ticks = 1490414594000;
            ticks *= 10000L;
            //ticks += 621355968000000000L;
            ticks += 63628076000000000L;
            DateTime a = new DateTime(ticks);
            DateTime b = new DateTime(2017, 04, 25);
            DateTime c = new DateTime(621355968000000000L);
            c = c.AddHours(9);
            Console.WriteLine((b.Ticks - a.Ticks).ToString());
            Console.ReadKey();*/

            //String key = GetWeb("http://localhost:8080/HouseholdData/UserCheck", "GID=103311668963248489400&NAME=黄淳皣&EMAIL=nowonbun@gmail.com");
            //String key = GetWeb("http://localhost:8080/HouseholdData/GetHouseholdList", "GID=103311668963248489400&YEAR=2016&MONTH=2");
            //String key = GetWeb("http://192.168.0.2:8080/HouseholdData/CheckUser", "GID=103311668963248489401");
            //String key = GetWeb("http://localhost:8080/HouseholdData/GetHouseholdList2", "GID=103311668963248489400&YEAR=2016&MONTH=2&CATEGORY=020");
            var tt = new Dictionary<String, Int32>() { { "a",0  } };
            String json = JsonConvert.SerializeObject(tt);
            var base64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            
            String key = GetWeb("http://localhost/Household/GetMaster.php","p=SY"+base64 );
            key = key.Substring(2);
            byte[] data = System.Convert.FromBase64String(key);
            key = Encoding.UTF8.GetString(data);
            Console.WriteLine(key);

            Dictionary<String, Object> ret = JsonConvert.DeserializeObject<Dictionary<String, Object>>(key);
            Console.WriteLine(key);
            //String data = GetData(key);
            //Console.WriteLine(data);
            
            Console.ReadLine();
        }
        public static string GetWeb(String url, String param = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (param != null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(param);
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
        public static String GetData(string key)
        {
            return GetWeb("http://192.168.0.2:8080/DataTransfer/Pop", "CODE=" + key);
        }
    }
}
