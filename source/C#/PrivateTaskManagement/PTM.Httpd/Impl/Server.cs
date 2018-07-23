using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using PTM.Httpd.Util;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Ionic.Zip;

namespace PTM.Httpd.Impl
{
    class Server : Socket, IServer, IDisposable
    {
        private class SessionId
        {
            public String id;
            public DateTime expire;
        }
        private Dictionary<String2, Action<Request, Response>> _method_list = new Dictionary<String2, Action<Request, Response>>();
        private Dictionary<SessionId, Dictionary<String, Object>> _session_map = new Dictionary<SessionId, Dictionary<String, object>>();
        private Func<String2, WebSocketNode> _websocket_method_list;
        private List<WebSocket> socketlist = new List<WebSocket>();
        private String _rootpath = null;
        private Dictionary<String2, String2> zipmap = new Dictionary<String2, String2>();
        private String _default = null;
        public Server(int port) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
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
                            try
                            {
                                String2 buffer = new String2(Define.BUFFER_SIZE);
                                _client.Receive(buffer.ToBytes(), buffer.Length, SocketFlags.None);
                                if (buffer.IsEmpty())
                                {
                                    _client.Close();
                                    return;
                                }
                                Request req = new Request(this, buffer);
                                if (req.IsWebSocket())
                                {
                                    socketlist.Add(new WebSocket(client, this, req, _websocket_method_list));
                                    return;
                                }
                                Response res = new Response(this, req);
                                String2 url = req.Uri;
                                if (_default != null && url.ToString().Equals("/"))
                                {
                                    url += _default;
                                }

                                if (_method_list.ContainsKey(url))
                                {
                                    _method_list[url](req, res);
                                }
                                else if (zipmap.ContainsKey(url))
                                {
                                    String extension = url.SubString(url.Length - 4, 4).ToUpper().ToString();
                                    if (".JS".Equals(extension.Substring(1)))
                                    {
                                        res.ContextType = "text/javascript; charset=UTF-8";
                                    }
                                    else if (".CSS".Equals(extension))
                                    {
                                        res.ContextType = "text/css; charset=UTF-8";
                                    }
                                    res.Body = zipmap[url];
                                }
                                else if (_rootpath != null)
                                {
                                    string filepath = _rootpath + url.ToString();
                                    if (File.Exists(filepath))
                                    {
                                        res.ReadFile(filepath);
                                    }
                                }
                                String2 sendbuffer = res.View();
                                client.Send(sendbuffer.ToBytes(), sendbuffer.Length, SocketFlags.None);
                                ThreadPool.QueueUserWorkItem((_) =>
                                {
                                    Thread.Sleep(5000);
                                    _client.Close();
                                });
                            }
                            catch (Exception e)
                            {
                                if (_client != null)
                                {
                                    _client.Dispose();
                                }
                                Console.WriteLine(e);
                            }
                        }, client);
                    }
                    catch (Exception e)
                    {
                        if (client != null)
                        {
                            client.Dispose();
                        }
                        Console.WriteLine(e);
                    }
                }
            });
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

        public void SetZip(String file)
        {
            using (ZipFile zip = new ZipFile(file))
            {
                foreach (var z in zip)
                {
                    using (var reader = z.OpenReader())
                    {
                        int length = (int)reader.Length;
                        String2 temp = new String2(length);
                        reader.Read(temp.ToBytes(), 0, length);
                        String2 f = z.FileName.Replace("\\", "/");
                        this.zipmap.Add("/" + f, temp);
                    }
                }
            }
        }

        public String2 GetZipFile(String key)
        {
            if (this.zipmap.ContainsKey(key))
            {
                return this.zipmap[key];
            }
            return null;
        }

        public void SetDefaultFile(String path)
        {
            this._default = path;
        }

        public void SetWebSocket(Func<String2, WebSocketNode> method)
        {
            this._websocket_method_list = method;
        }

        public void RemoveWebSocket(WebSocket socket)
        {
            socketlist.Remove(socket);
        }

        public void Send(int opcode, String2 data)
        {
            socketlist.AsParallel().ForAll(_client =>
            {
                _client.Send(opcode, data);
            });
        }

        public Dictionary<String, Object> GetSession(String key)
        {
            SessionId id = null;
            foreach (var obj in _session_map.Keys)
            {
                if (obj.id.Equals(key))
                {

                    if (obj.expire.Ticks > DateTime.Now.Ticks)
                    {
                        id = obj;
                    }
                    else
                    {
                        _session_map.Remove(obj);
                    }
                    break;
                }
            }
            if (id == null)
            {
                id = new SessionId();
                id.id = key;
                id.expire = DateTime.Now.AddHours(1);
                _session_map.Add(id, new Dictionary<String, object>());
            }
            return _session_map[id];
        }
    }
}
