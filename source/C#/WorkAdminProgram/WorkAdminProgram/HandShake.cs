using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using WorkServer;
using System.Linq;

namespace WorkAdminProgram
{
    class HandShake
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(HandShake));
        private String2 header;
        private IDictionary<String2, String2> option;
        public HandShake(String2 data)
        {
            String2[] buffer = data.Split(String2.CRLF + String2.CRLF)[0].Split(String2.CRLF);
            if (buffer.Length < 1)
            {
                throw new FormatException();
            }
            header = buffer[0];
            option = (from step1 in buffer
                     where !header.Equals(step1)
                     let step2 = step1.Split(":")
                     where step2.Length >= 2
                     let key = step2[0].Trim().ToUpper()
                     let value = step1.Replace(step2[0], "").Replace(":","").Trim()
                     select new { k = key, v = value })
                    .ToDictionary(i => i.k, i => i.v);
        }

        public static implicit operator HandShake(String2 data)
        {
            return new HandShake(data);
        }

        public static implicit operator String2(HandShake data)
        {
            return data.ToString();
        }

        public override String ToString()
        {
            return header.ToString();
        }

        public String2 this[String2 key]{
            get
            {
                key = key.ToUpper();
                if (option.ContainsKey(key))
                {
                    return option[key];
                }
                return null;
            }
        }
    }
}
