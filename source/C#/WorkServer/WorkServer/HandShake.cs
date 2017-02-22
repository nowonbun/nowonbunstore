using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkServer
{
    class HandShake
    {
        private String2 header;
        private String2 data;
        private IDictionary<String2, String2> map = new Dictionary<String2, String2>();
        public HandShake(String2 data)
        {
            this.data = data;
            String2[] temp = data.Split(String2.CRLF + String2.CRLF)[0].Split(String2.CRLF);
            if (temp.Length < 1)
            {
                throw new FormatException("Socket format is wrong.");
            }
            header = temp[0];
            for (int i = 1; i < temp.Length; i++)
            {
                String2[] buffer = temp[i].Split(":");
                if (buffer.Length == 2)
                {
                    map.Add(buffer[0].Replace(":", "").Trim(), buffer[1].Replace(String2.CRLF, "").Trim());
                }
                else if (buffer.Length > 2)
                {
                    String2 val = "";
                    for (int j = 1; j < buffer.Length; j++)
                    {
                        val += buffer[j];
                    }
                    map.Add(buffer[0].Replace(":", "").Trim(), val.Replace(String2.CRLF, "").Trim());
                }
            }
        }
        public static implicit operator HandShake(String2 data)
        {
            return new HandShake(data);
        }
        public String2 Header
        {
            get { return this.header; }
        }
        public String2 Get(String2 key)
        {
            if (map.ContainsKey(key))
            {
                return map[key];
            }
            return null;
        }

        public WorkServer GetServer()
        {
            return null;
        }

        public override String ToString()
        {
            return data.ToString();
        }
    }
}
