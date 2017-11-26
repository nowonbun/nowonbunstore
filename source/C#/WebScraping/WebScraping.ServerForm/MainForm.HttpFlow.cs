using System;
using System.Collections.Generic;
using WebScraping.WebServer;
using WebScraping.Library.Config;
using Newtonsoft.Json;

namespace WebScraping.ServerForm
{
    partial class MainForm
    {
        private Dictionary<String, Scraper> scraperlist = new Dictionary<string, Scraper>();
        private IServer server;

        public class SocketNode
        {
            public String key { get; set; }
            public String data { get; set; }
        }
        public void InitializeFlow()
        {
            server = ServerFactory.NewInstance(ServerInfo.GetPort());
            server.SetRootPath(ServerInfo.GetWebRoot());
            /*IWebHttpServer http = factory.WebServer;
            IWebSocketServer socket = factory.WebSocketServer;*/

            server.Set("/ControllView", (req, res) =>
            {
                res.SetHeader("Content-Type", "text/html; charset=utf-8");
                res.ReadFile(ServerInfo.GetWebRoot() + "\\index.html");
            });
            server.Set("/Jquery", (req, res) =>
            {
                res.SetHeader("Content-Type", "text/javascript; charset=UTF-8");
                res.ReadFile(ServerInfo.GetWebRoot() + "\\jquery-3.2.1.min.js");
            });
            server.Set("/Css", (req, res) =>
            {
                res.SetHeader("Content-Type", "text/css;");
                res.ReadFile(ServerInfo.GetWebRoot() + "\\common.css");
            });
            server.Set("/Javascript", (req, res) =>
            {
                res.SetHeader("Content-Type", "text/css;");
                res.ReadFile(ServerInfo.GetWebRoot() + "\\common.js");
            });

            IDictionary<String, Action<String, WebSocketNode>> socketmethod = new Dictionary<String, Action<String, WebSocketNode>>();
            socketmethod.Add("init", (data, node) =>
            {
                logger.Debug("init");
                //scraperlist;
                List<Object> ret = new List<object>();
                foreach (var s in scraperlist)
                {
                    ret.Add(s.Value.Parameter);
                }
                SocketNode message = new SocketNode()
                {
                    key = "init",
                    data = JsonConvert.SerializeObject(ret)
                };
                node.OPCode = Opcode.MESSAGE;
                node.Message = JsonConvert.SerializeObject(message);
                node.Broadcast = false;
            });
            socketmethod.Add("log", (data, node) =>
            {
                logger.Debug("log");
                node.OPCode = Opcode.MESSAGE;
                logger.Info(data);
            });
            socketmethod.Add("start", (data, node) =>
            {
                StartScraper(data);
            });
            /*socketmethod.Add("testremove", (node) =>
            {
                Console.WriteLine("testremove");
                node.OPCode = Opcode.MESSAGE;
                node.Message = JsonConvert.SerializeObject(new SocketNode() { key = "remove", data = "test1" });
                node.Broadcast = true;
            });*/

            server.SetWebSocket((data, opcode) =>
            {
                WebSocketNode ret = new WebSocketNode() { OPCode = Opcode.MESSAGE };
                String message = data.ToString();
                try
                {
                    SocketNode node = JsonConvert.DeserializeObject<SocketNode>(message);
                    if (!socketmethod.ContainsKey(node.key))
                    {
                        throw new Exception("not method");
                    }
                    socketmethod[node.key](node.data, ret);
                }
                catch (Exception e)
                {
                    logger.Error(e.ToString());
                    ret.Broadcast = false;
                    ret.Message = JsonConvert.SerializeObject(new SocketNode() { key = "error" });
                }
                return ret;
            });
        }

        private String StartScraper(String param)
        {
            this.logger.Info("Start scraper param : " + param);
            Scraper scraper = new Scraper(param, path);
            String key = scraper.Run();
            scraperlist.Add(key, scraper);
            server.SendWebSocket(new WebSocketNode()
            {
                OPCode = Opcode.MESSAGE,
                Broadcast = true,
                Message = JsonConvert.SerializeObject(new SocketNode() { key = "insert", data = JsonConvert.SerializeObject(scraper.Parameter) })
            });
            return key;
        }
    }
}
