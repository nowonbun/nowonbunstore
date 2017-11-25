using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WebScraping.WebServer.Util;
using System.IO;
using WebScraping.Library.Log;

namespace WebScraping.WebServer.Impl
{
    class Server : Socket, IServer, IDisposable
    {
        private Logger logger;
        private Dictionary<String2, Action<Request, Response>> _method_list = new Dictionary<String2, Action<Request, Response>>();
        private Func<String2, Opcode, WebSocketNode> _websocket_method_list;
        private List<WebSocket> socketlist = new List<WebSocket>();
        private String _rootpath = null;
        public Server(int port) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
            Bind(new IPEndPoint(IPAddress.Any, port));
            Listen(20);
        }
        public void ServerStart()
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                while (true)
                {
                    Socket client = null;
                    try
                    {
                        client = Accept();
                        ThreadPool.QueueUserWorkItem((temp_client) =>
                        {
                            var _client = temp_client as Socket;
                            String2 buffer = new String2(Define.BUFFER_SIZE);
                            _client.Receive(buffer.ToBytes(), buffer.Length, SocketFlags.None);
                            if (buffer.ToBytes()[0] == 0)
                            {
                                logger.Debug("not Byte data..");
                                //TODO : Bug??
                                _client.Close();
                                return;
                            }
                            Request req = new Request(buffer);
                            Console.WriteLine(req.Uri);
                            if (req.IsWebSocket())
                            {
                                socketlist.Add(new WebSocket(client, this, req, _websocket_method_list));
                                return;
                            }
                            Response res = new Response();
                            Console.WriteLine(req.Uri);
                            if (_method_list.ContainsKey(req.Uri))
                            {
                                _method_list[req.Uri](req, res);
                            }
                            else if (_rootpath != null)
                            {
                                string filepath = _rootpath + req.Uri.ToString();
                                if (File.Exists(filepath))
                                {
                                    res.ReadFile(filepath);
                                }
                            }
                            String2 sendbuffer = TransResponse(res);
                            client.Send(sendbuffer.ToBytes(), sendbuffer.Length, SocketFlags.None);
                            _client.Close();
                        }, client);
                    }
                    catch (Exception e)
                    {
                        if (client != null)
                        {
                            client.Dispose();
                        }
                        throw e;
                    }
                }
            });
        }
        private String2 TransResponse(Response res)
        {
            String2 buffer = new String2(0);
            buffer += res.Version + " " + res.State + " " + res.StateComment + String2.CRLF;
            foreach (var h in res.Headers)
            {
                buffer += h.Key + ": " + h.Value + String2.CRLF;
            }
            buffer += String2.CRLF;
            buffer += res.Body;
            buffer += String2.CRLF + String2.CRLF;
            return buffer;
        }
        public void Set(String2 key, Action<Request, Response> method)
        {
            if (_method_list.ContainsKey(key))
            {
                _method_list.Remove(key);
            }
            _method_list.Add(key, method);
        }
        public void SetRootPath(String path)
        {
            _rootpath = path;
        }
        public void SetWebSocket(Func<String2, Opcode, WebSocketNode> method)
        {
            this._websocket_method_list = method;
        }
        public void Send(WebSocketNode node)
        {
            for (int i = 0; i < socketlist.Count; i++)
            {
                socketlist[i].Send((int)node.OPCode, node.Message);
            }
        }
        public void RemoveSocket(WebSocket socket)
        {
            socketlist.Remove(socket);
        }
    }
}
