using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace WorkServer
{
    class ServerImpl : Socket, IDisposable, Server
    {
        public event Action<Client> Acception;

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
                    Client client = null;
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

        private new Client Accept()
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
