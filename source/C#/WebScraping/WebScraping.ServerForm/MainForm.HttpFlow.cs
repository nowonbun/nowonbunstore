using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            HttpFactory factory = HttpFactory.NewInstance(ServerInfo.GetPort(), ServerInfo.GetWebsocketPort());
            IWebHttpServer http = factory.WebServer;
            IWebSocketServer socket = factory.WebSocketServer;

            http.Get("/ControllView", (req, res) =>
            {
                res.Headers.Add("Content-Type", "text/html; charset=utf-8");
                return ServerInfo.GetWebRoot() + "\\index.html";
            });
            http.Get("/Jquery", (req, res) =>
            {
                res.Headers.Add("Content-Type", "text/javascript; charset=UTF-8");
                return ServerInfo.GetWebRoot() + "\\jquery-3.2.1.min.js";
            });
            http.Get("/Css", (req, res) =>
            {
                res.Headers.Add("Content-Type", "text/css;");
                return ServerInfo.GetWebRoot() + "\\common.css";
            });
            http.Get("/Javascript", (req, res) =>
            {
                res.Headers.Add("Content-Type", "text/css;");
                return ServerInfo.GetWebRoot() + "\\common.js";
            });

            IDictionary<String, Func<String>> socketmethod = new Dictionary<String, Func<String>>();
            socketmethod.Add("init", () =>
            {
                return JsonConvert.SerializeObject(new List<Object>() {
                    new { Key = "test1", Code = "code1", Id = "id1", Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Pingtime = ""},
                    new { Key = "test2", Code = "code2", Id = "id2", Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Pingtime = "" } });
            });
            socketmethod.Add("testremove", () =>
            {
                String buffer = JsonConvert.SerializeObject(new SocketNode() { key = "remove", data = "test1" });
                socket.Send(new WebSocketNode() { OPCode = Opcode.MESSAGE, Message = Encoding.UTF8.GetBytes(buffer) });
                return "";
            });

            socket.Get((data) =>
            {
                WebSocketNode ret = new WebSocketNode() { OPCode = Opcode.MESSAGE };
                String message = Encoding.UTF8.GetString(data);
                try
                {
                    SocketNode node = JsonConvert.DeserializeObject<SocketNode>(message);
                    if (!socketmethod.ContainsKey(node.key))
                    {
                        throw new Exception("not method");
                    }
                    node.data = socketmethod[node.key]();
                    ret.SetMessage(JsonConvert.SerializeObject(node));
                }
                catch(Exception e)
                {
                    logger.Error(e.ToString());
                    ret.SetMessage(JsonConvert.SerializeObject(new SocketNode() { key = "error" }));
                }
                return ret;
            });
        }
    }
}
