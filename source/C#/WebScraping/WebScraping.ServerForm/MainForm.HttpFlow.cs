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
            server.Set("/Ping", (req, res) =>
            {
                if (req.QueryString.ContainsKey("Code"))
                {
                    String code = req.QueryString["Code"].ToString();
                    if (scraperlist.ContainsKey(code))
                    {
                        scraperlist[code].Parameter.Pingtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        server.SendWebSocket(new WebSocketNode()
                        {
                            OPCode = Opcode.MESSAGE,
                            Broadcast = true,
                            Message = JsonConvert.SerializeObject(new SocketNode() { key = "ping", data = JsonConvert.SerializeObject(scraperlist[code].Parameter) })
                        });
                    }
                }
            });
            server.Set("/Scrap", (req, res) =>
            {
                if (!req.QueryString.ContainsKey("Code"))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                if (!req.QueryString.ContainsKey("Id"))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                if (!req.QueryString.ContainsKey("Pw"))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                String param = "Code=" + req.QueryString["Code"].ToString() + "&Id=" + req.QueryString["Id"].ToString() + "&Pw=" + req.QueryString["Pw"].ToString();
                this.logger.Info("Start scraper param : " + param);
                Scraper scraper = new Scraper(param);
                String key = scraper.Run();
                scraperlist.Add(key, scraper);
                server.SendWebSocket(new WebSocketNode()
                {
                    OPCode = Opcode.MESSAGE,
                    Broadcast = true,
                    Message = JsonConvert.SerializeObject(new SocketNode() { key = "insert", data = JsonConvert.SerializeObject(scraper.Parameter) })
                });
                res.StateOK();
            });
            server.Set("/EndScrap", (req, res) =>
            {
                if (!req.QueryString.ContainsKey("Code"))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                this.logger.Info("EndScrap");
                String code = req.QueryString["Code"].ToString();
                if (!scraperlist.ContainsKey(code))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                this.logger.Debug("Exit Code = " + code);
                server.SendWebSocket(new WebSocketNode()
                {
                    OPCode = Opcode.MESSAGE,
                    Broadcast = true,
                    Message = JsonConvert.SerializeObject(new SocketNode() { key = "remove", data = JsonConvert.SerializeObject(scraperlist[code].Parameter) })
                });
                scraperlist.Remove(code);
                res.StateOK();
                return;
            });
            server.Set("/AbortScrap", (req, res) =>
            {
                if (!req.QueryString.ContainsKey("Code"))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                this.logger.Info("EndScrap");
                String code = req.QueryString["Code"].ToString();
                if (!scraperlist.ContainsKey(code))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                this.logger.Debug("Exit Code = " + code);
                scraperlist[code].Kill();
                server.SendWebSocket(new WebSocketNode()
                {
                    OPCode = Opcode.MESSAGE,
                    Broadcast = true,
                    Message = JsonConvert.SerializeObject(new SocketNode() { key = "remove", data = JsonConvert.SerializeObject(scraperlist[code].Parameter) })
                });
                scraperlist.Remove(code);
                res.StateOK();
            });
            server.Set("/RestartScrap", (req, res) =>
            {
                if (!req.QueryString.ContainsKey("Code"))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                this.logger.Info("RestartScrap");
                String code = req.QueryString["Code"].ToString();
                if (!scraperlist.ContainsKey(code))
                {
                    res.SetResponseCode(400, "Bad Request");
                    return;
                }
                scraperlist[code].Kill();
                String param = "Code=" + scraperlist[code].Parameter.Code + "&Id=" + scraperlist[code].Parameter.Id + "&Pw=" + scraperlist[code].Parameter.Pw;
                Scraper scraper = new Scraper(param);
                scraperlist[code] = scraper;
                scraper.Run(code, false);
                server.SendWebSocket(new WebSocketNode()
                {
                    OPCode = Opcode.MESSAGE,
                    Broadcast = true,
                    Message = JsonConvert.SerializeObject(new SocketNode() { key = "restart", data = JsonConvert.SerializeObject(scraperlist[code].Parameter) })
                });
                res.StateOK();
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
            Scraper scraper = new Scraper(param);
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
