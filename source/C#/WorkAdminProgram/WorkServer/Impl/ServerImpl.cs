using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace WorkServer
{
    class ServerImpl : Socket, IDisposable, IServer
    {
        public event Action<IClient> Acception;

        public int Port { get; set; }
        public int Timeout { get; set; }

        public ServerImpl(int port, int timeout)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            Bind(new IPEndPoint(IPAddress.Any, port));
            Listen(20);
            this.Port = port;
            this.Timeout = timeout;
        }

        public void ServerStart()
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                while (true)
                {
                    IClient client = null;
                    try
                    {
                        client = Accept();
                        Acception(client);
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

        private new IClient Accept()
        {
            ClientImpl client = base.Accept();
            client.GetStream().ReadTimeout = this.Timeout;
            return client;
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
