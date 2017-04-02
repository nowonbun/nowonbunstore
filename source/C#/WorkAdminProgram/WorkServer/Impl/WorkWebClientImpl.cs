using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WorkServer
{
    partial class WorkWebClientImpl : IWorkWebClient
    {
        private IClient client;
        private IDictionary<String, String> Option = new Dictionary<String, String>();

        public String Path { get; set; }
        public String Parameter { get; set; }
        public Exception LastException { get; set; }

        class HeaderOption
        {
            IDictionary<String, String> option = new Dictionary<String, String>();
            public static HeaderOption Create()
            {
                return new HeaderOption();
            }
            public void AddOption(String key, String value)
            {
                option.Add(key, value);
            }
            public IDictionary<String, String> ToResult()
            {
                return option;
            }
        }

        public WorkWebClientImpl(IClient client)
        {
            this.client = client;
        }

        public void Initialize(String2 header)
        {
            try
            {
                String2[] buffer = header.Split(" ");
                if (buffer.Length != 3 || buffer[1] == null)
                {
                    throw new FormatException();
                }
                String temp = buffer[1].ToString().Trim();
                int pos = temp.IndexOf("?");
                if (pos < 0)
                {
                    this.Path = temp;
                    return;
                }
                this.Path = temp.Substring(0, pos);
                this.Parameter = temp.Substring(pos + 1, temp.Length - (pos + 1));
               
            }
            catch (Exception e)
            {
                throw new FormatException(null, e);
            }
        }

        public void Run(ResponeCode code, FileInfo file, bool fileoption = false)
        {
            try
            {
                HeaderOption option = HeaderOption.Create();
                if (fileoption)
                {
                    option.AddOption("Content-Type", "multipart/formed-data");
                    option.AddOption("Content-Disposition", "attachment; filename=" + file.Name);
                    option.AddOption("Length", file.Length.ToString());
                }
                else
                {
                    option.AddOption("Content-Type", "text/html");
                }
                byte[] header = BuildHeader(code, option);
                client.Send(header);
                if (file != null && file.Exists)
                {
                    byte[] data = GetFile(file);
                    client.Send(data);
                }
            }
            catch (Exception e)
            {
                LastException = e;
                client.Send(BuildHeader(ResponeCode.CODE500));
            }
            finally
            {
                client.Close();
            }
        }

        private byte[] BuildHeader(ResponeCode code, HeaderOption option = null)
        {
            String2 ret = new String2(Encoding.UTF8);
            if (object.Equals(code, ResponeCode.CODE200))
            {
                ret += "HTTP/1.1 200 OK" + String2.CRLF;
            }
            else
            {
                ret += "HTTP/1.1 " + ((int)code) + " NG" + String2.CRLF;
            }
            if (option != null)
            {
                foreach (String key in option.ToResult().Keys)
                {
                    ret += key + ": " + option.ToResult()[key] + String2.CRLF;
                }
            }
            ret += "Keep-Alive: timeout=15, max=93" + String2.CRLF;
            ret += "Connection: Keep-Alive" + String2.CRLF + String2.CRLF;
            return ret.ToBytes();
        }

        private byte[] GetFile(FileInfo file)
        {
            byte[] data = new byte[file.Length];
            using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                stream.Read(data, 0, data.Length);
            }
            return data;
        }
    }
}
