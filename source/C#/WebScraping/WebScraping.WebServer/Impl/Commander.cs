using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebScraping.WebServer.Impl
{
    class Commander
    {
        private ClientSocket socket;
        public Commander(ClientSocket socket)
        {
            this.socket = socket;
        }

        public void Run(String cmd, Stream stream)
        {
            byte[] header = null;
            if ("/ControllView".Equals(cmd))
            {
                var app_dir = Path.GetDirectoryName(socket.Path);
                byte[] data = GetHtmlFile(app_dir + "\\Web\\index.html");
                if (data != null)
                {
                    header = CreateResponse(200, "OK", 1);
                    stream.Write(header, 0, header.Length);
                    stream.Write(data, 0, data.Length);
                    return;
                }
            }
            else if ("/jquery-3.2.1.min.js".Equals(cmd))
            {
                var app_dir = Path.GetDirectoryName(socket.Path);
                byte[] data = GetHtmlFile(app_dir + "\\Web\\jquery-3.2.1.min.js");
                if (data != null)
                {
                    header = CreateResponse(200, "OK", 2);
                    stream.Write(header, 0, header.Length);
                    stream.Write(data, 0, data.Length);
                    return;
                }
            }
            header = CreateResponse(501, "Not Implemented");
        }
        private byte[] GetHtmlFile(String filepath)
        {
            FileInfo file = new FileInfo(filepath);
            if (!file.Exists)
            {
                return null;
            }
            byte[] data = new byte[file.Length];
            using (FileStream stream = file.OpenRead())
            {
                stream.Read(data, 0, data.Length);
                return data;
            }
        }
        private byte[] CreateResponse(int code, String msg, int type = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HTTP/1.1 ").Append(code).Append(" ").Append(msg).AppendLine();
            if (type == 1)
            {
                sb.Append("Content-Type:text/html; charset=utf-8");
            }
            else if (type == 2)
            {
                sb.Append("content-type:text/javascript; charset=UTF-8");
            }
            sb.AppendLine("Connection: close").AppendLine();
            String ret = sb.ToString();
            return Encoding.Default.GetBytes(ret);
        }
    }
}
