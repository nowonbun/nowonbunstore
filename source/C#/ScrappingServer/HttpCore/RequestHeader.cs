using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ScrappingHttpCore
{
    class RequestHeader : Dictionary<String, String>, IHeader
    {
        private bool state = false;
        public new String this[String parameterName]
        {
            get
            {
                if (!ContainsKey(parameterName))
                {
                    return "";
                }
                return base[parameterName];
            }
        }
        public bool GetState()
        {
            return state;
        }
        public RequestHeader(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            String data;
            String parameterName;
            StringBuilder buffer = new StringBuilder();
            while (!String.IsNullOrEmpty(data = reader.ReadLine()))
            {
                if (data.IndexOf("GET") != -1 || data.IndexOf("POST") != -1 || data.IndexOf("HTTP/1.1") != -1)
                {
                    data = data.Replace("GET", "").Replace("POST", "").Replace("HTTP/1.1","").Trim();
                    bool fileFlg = true;
                    bool parameterFlg = false;
                    char[] value = data.ToCharArray();
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (fileFlg)
                        {
                            if (i >= 2)
                            {
                                return;
                            }
                        }
                        if (object.Equals('?', value[i]))
                        {
                            parameterFlg = true;
                            fileFlg = false;
                            continue;
                        }
                        if (!parameterFlg)
                        {
                            continue;
                        }
                        if (object.Equals(' ', value[i]))
                        {
                            parameterFlg = false;
                        }
                        if (!object.Equals('=', value[i]))
                        {
                            buffer.Append(value[i]);
                            continue;
                        }
                        parameterName = buffer.ToString().ToUpper();
                        buffer.Clear();
                        for (++i; i < value.Length; i++)
                        {
                            if (object.Equals('&', value[i]) || object.Equals(' ', value[i]))
                            {
                                break;
                            }
                            buffer.Append(value[i]);
                        }
                        Add(parameterName, buffer.ToString());
                        buffer.Clear();
                    }
                    state = true;
                    break;
                }
            }
        }
    }
}
