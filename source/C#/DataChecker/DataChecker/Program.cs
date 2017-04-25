using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.IO;

namespace DataChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //String key = GetWeb("http://localhost:8080/HouseholdData/UserCheck", "GID=103311668963248489400&NAME=黄淳皣&EMAIL=nowonbun@gmail.com");
            String key = GetWeb("http://localhost:8080/HouseholdData/GetHouseholdList", "GID=103311668963248489400&YEAR=2016&MONTH=2");
            Console.WriteLine(key);
            String data = GetData(key);
            Console.WriteLine(data);

            Console.ReadKey();
        }
        public static string GetWeb(String url, String param)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
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
