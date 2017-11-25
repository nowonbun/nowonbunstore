using System;
using System.Collections.Generic;
using WebScraping.WebServer;
using WebScraping.Library.Config;
using Newtonsoft.Json;

namespace WebScraping.ServerForm
{
    partial class MainForm
    {
        public class SocketNode
        {
            public String key { get; set; }
            public String data { get; set; }
        }
        public void InitializeFlow()
        {
            IServer server = ServerFactory.NewInstance(ServerInfo.GetPort());
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

            IDictionary<String, Action<WebSocketNode>> socketmethod = new Dictionary<String, Action<WebSocketNode>>();
            socketmethod.Add("init", (node) =>
            {
                Console.WriteLine("init");
                SocketNode message = new SocketNode()
                {
                    key = "init",
                    data = JsonConvert.SerializeObject(new List<Object>() {
                    new { Key = "test1", Code = "code1", Id = "id1", Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Pingtime = ""},
                    new { Key = "test2", Code = "code2", Id = "id2", Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Pingtime = "" } })
                };
                node.OPCode = Opcode.MESSAGE;
                node.Message = JsonConvert.SerializeObject(message);
                node.Broadcast = false;
            });

            socketmethod.Add("testremove", (node) =>
            {
                Console.WriteLine("testremove");
                node.OPCode = Opcode.MESSAGE;
                node.Message = JsonConvert.SerializeObject(new SocketNode() { key = "remove", data = "test1" });
                node.Broadcast = true;
            });

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
                    socketmethod[node.key](ret);
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
    }
}
