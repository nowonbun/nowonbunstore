using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using WebScraping.Library.Log;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace WebScraping.WebServer.Impl
{
    class ClientSocket : TcpClient, IClientSocket
    {
        private String[] _header;
        private String _response;
        private Logger logger;

        public String[] Header
        {
            get { return this._header; }
        }
        public String path;
        public ServerSocket server;

        public ClientSocket(ServerSocket server, Socket client, String path)
        {
            this.logger = LoggerBuilder.Init().Set(this.GetType());
            this.server = server;
            this.path = path;
            base.Client = client;
        }
        public void Run()
        {
            byte[] buffer = new byte[4096];
            using (Stream stream = GetStream())
            {
                stream.Read(buffer, 0, buffer.Length);
                String msg = Encoding.Default.GetString(TrimByte(buffer));
                logger.Debug(msg);
                try
                {
                    _header = GetHeader(msg);
                    Command(_header[1], stream);
                }
                catch (Exception e)
                {
                    byte[] rep = CreateResponse(400, "Bad Request");
                    stream.Write(rep, 0, rep.Length);
                    logger.Error(e.ToString());
                }
            }
        }
        public void Command(String msg, Stream stream)
        {
            byte[] header = null;
            logger.Debug(msg);
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
                var app_dir = Path.GetDirectoryName(this.path);
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
                var app_dir = Path.GetDirectoryName(this.path);
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
                this.logger.Info("Call scraping...");
                this.server.StartScraper(param);
                header = CreateResponse(200, "OK", 1);
                stream.Write(header, 0, header.Length);
                return;
            }
            else if ("LIST".Equals(cmd))
            {
                this.logger.Info("Call data list...");
                IList<Scraper> scraperlist = this.server.GetScraperList();
                IList<Parameter> jsonlist = scraperlist.Select(node => { return node.Parameter; }).ToList();
                String json = JsonConvert.SerializeObject(jsonlist);
                this.logger.Debug(json);
                byte[] data = Encoding.UTF8.GetBytes(json);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                stream.Write(data, 0, data.Length);
                return;
            }
            else if ("PING".Equals(cmd))
            {
                this.logger.Info("PING");
                var temp = CreateParam(param);
                String code = temp["CODE"];
                this.logger.Debug("Ping Code = " + code);
                this.server.PingScraper(code);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                return;
            }
            else if ("ENDSCRAP".Equals(cmd))
            {
                this.logger.Info("EndScrap");
                var temp = CreateParam(param);
                String code = temp["CODE"];
                this.logger.Debug("Exit Code = " + code);
                this.server.RemoveScraper(code);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                return;
            }
            else if ("Restart".Equals(cmd))
            {

            }
            else if ("LOG".Equals(cmd))
            {
                logger.Info("Javascript logger : " + param);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                return;
            }
            header = CreateResponse(501, "Not Implemented");
            stream.Write(header, 0, header.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="type">0: No Header, 1: text/html, 2: text/javascript</param>
        /// <returns></returns>
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
        private String[] GetHeader(String header)
        {
            if (header.Length < 0)
            {
                throw new Exception("Http header error length - 0");
            }
            int pos = header.IndexOf("\r\n");
            if (pos < 0)
            {
                //throw new Exception("Http header error type - \\r\\n");
                pos = header.Length - 1;
            }
            if (pos < 0)
            {
                return header.Split(' ');
            }
            header = header.Substring(0, pos);
            return header.Split(' ');
        }
        private byte[] TrimByte(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    byte[] buffer = new byte[i];
                    Array.Copy(data, buffer, i);
                    return buffer;
                }
            }
            return data;
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
        private IDictionary<String, String> CreateParam(String param)
        {
            IDictionary<String, String> ret = new Dictionary<String, String>();
            String[] temp = param.Split('&');
            foreach (String t in temp)
            {
                String[] buffer = t.Split('=');
                String key = buffer[0].ToUpper();

                String data = buffer[1];
                ret.Add(key, data);
            }

            return ret;
        }
    }
}
