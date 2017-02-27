using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WorkServer
{
    class HandShake
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(HandShake));
        private String2 header;
        private String2 data;
        private IDictionary<String2, String2> map = new Dictionary<String2, String2>();
        public HandShake(String2 data)
        {
            this.data = data;
            logger.Debug(data);
            String2[] temp = data.Split(String2.CRLF + String2.CRLF)[0].Split(String2.CRLF);
            logger.Debug(temp.Length);
            if (temp.Length < 1)
            {
                throw new FormatException("Socket format is wrong.");
            }
            logger.Debug("DEBUG!!");
            header = temp[0];
            for (int i = 1; i < temp.Length; i++)
            {
                String2[] buffer = temp[i].Split(":");
                if (buffer.Length == 2)
                {
                    String2 key = buffer[0].Replace(":", "").Trim().ToUpper();
                    String2 val = buffer[1].Replace(String2.CRLF, "").Trim();
                    map.Add(key, val);
                }
                else if (buffer.Length > 2)
                {
                    String2 val = "";
                    for (int j = 1; j < buffer.Length; j++)
                    {
                        val += buffer[j];
                    }
                    String2 key = buffer[0].Replace(":", "").Trim().ToUpper();
                    val = val.Replace(String2.CRLF, "").Trim();
                    map.Add(key, val);
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
            key = key.ToUpper();
            if (map.ContainsKey(key))
            {
                return map[key];
            }
            return null;
        }

        public WorkServer ServerBuilder(Client client)
        {
            String2 type = Get(Define.PROTOCOL_CONNECTION);
            if (type == null)
            {
                throw new Exception("header errer");
            }
            type = type.ToUpper();
            if (type.Equals(Define.KEEP_ALIVE))
            {
                return new WebServer(client);
            }
            else if (type.Equals(Define.UPGRADE))
            {
                return new WebSocketServer(client);
            }
            throw new Exception("header errer");
        }

        public override String ToString()
        {
            return data.ToString();
        }
    }
}
