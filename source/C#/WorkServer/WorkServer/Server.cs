using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace WorkServer
{
    class Server : Socket
    {
        private static ILog logger = LogManager.GetLogger(typeof(Server));
        public event Action<Client> Acception;

        public Server(int port)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            Bind(new IPEndPoint(IPAddress.Any, port));
            Listen(20);
        }
        public static implicit operator Server(int port)
        {
            return new Server(port);
        }
        public void ServerStart()
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                while (true)
                {
                    Client client =null;
                    try
                    {
                        Acception(client = Accept());
                    }
                    catch (Exception e)
                    {
                        logger.Error(e);
                        if (client != null)
                        {
                            client.Dispose();
                        }
                    }
                }
            });
        }
    }
}
