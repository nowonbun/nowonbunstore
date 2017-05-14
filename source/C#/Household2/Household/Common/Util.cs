using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Household.Common
{
    public static class Util
    {
        public static String MD5HashCrypt(string val)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(val);
            bs = md5.ComputeHash(bs);
            StringBuilder buffer = new StringBuilder();
            foreach (byte b in bs)
            {
                buffer.Append(b.ToString("x2").ToLower());
            }
            return buffer.ToString();
        }
        public static bool GetPlusMinus(String code)
        {
            return String.Equals("1",code.Substring(2, 1));
        }

        public static DateTime TransDateTImeByJavaTicks(Int64 ticks)
        {
            ticks *= 10000L;
            //ticks += 621355968000000000L;
            ticks += 621356292000000000L;
            DateTime ret = new DateTime(ticks);
            return ret;
        }
    }
}