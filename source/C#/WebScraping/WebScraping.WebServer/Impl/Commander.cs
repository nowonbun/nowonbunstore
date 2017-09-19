using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WebScraping.WebServer.Impl
{
    class Commander
    {
        private ClientSocket socket;
        public Commander(ClientSocket socket)
        {
            this.socket = socket;
        }

        public void Run(String msg, Stream stream)
        {
            byte[] header = null;
            Console.WriteLine(msg);
            if (msg == null)
            {
                header = CreateResponse(501, "Not Implemented");
                stream.Write(header, 0, header.Length);
            }
            String[] buffer = msg.Split('?');
            if (buffer.Length < 1 || buffer[0].Length < 2)
            {
                header = CreateResponse(501, "Not Implemented");
                stream.Write(header, 0, header.Length);
            }
            String cmd = buffer[0].ToUpper().Substring(1);
            String param = null;
            if (buffer.Length > 1)
            {
                param = buffer[1];
            }
            
            if ("CONTROLLVIEW".Equals(cmd))
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
            else if ("JQUERY".Equals(cmd))
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
            else if ("SCRAP".Equals(cmd))
            {
                String key = System.Guid.NewGuid().ToString();
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo();
                process.StartInfo.FileName = "WebScraping.Scraper.exe";

                process.StartInfo.Arguments = key + " " + param;
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(socket.Path);
                process.Start();

                socket.Server.AddScraper(key, process);

                header = CreateResponse(200, "OK", 1);
                stream.Write(header, 0, header.Length);
                return;
            }
            header = CreateResponse(501, "Not Implemented");
            stream.Write(header, 0, header.Length);
            
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
