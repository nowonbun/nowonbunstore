using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebScraping.WebServer.Impl
{
    class ServerSocket : Socket, IServerSocket, IDisposable
    {
        private int port;
        private event Action<IClientSocket> acceptEvent;
        private Thread _thread;
        private bool live = true;

        public ServerSocket(int port) : base(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.IP)
        {
            this.port = port;
            base.Bind(new IPEndPoint(IPAddress.Any, port));
            base.Listen(100);
            acceptEvent += (c) => { };
            this._thread = new Thread(() =>
            {
                while (live)
                {
                    try
                    {
                        ClientSocket client = Accept();
                        client.Run();
                        acceptEvent(client);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log
                    }
                }
            });
        }
        public void SetAcceptEvent(Action<IClientSocket> e)
        {
            acceptEvent += e;
        }
        public void Run()
        {
            this._thread.Start();
        }
        public new void Dispose()
        {
            live = false;
            SendPingPong();
            base.Dispose();
        }
        private void SendPingPong()
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                s.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            }
        }
    }
}
