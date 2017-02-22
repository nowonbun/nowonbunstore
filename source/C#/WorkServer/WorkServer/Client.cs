using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace WorkServer
{
    class Client : TcpClient
    {
        private Server server = null;
        public Client(Socket sock)
        {
            this.Client = sock;
        }
        public Client SetServer(Server server)
        {
            this.server = server;
            return this;
        }
        public static implicit operator Client(Socket sock)
        {
            return new Client(sock);
        }
        public void Send(String2 data)
        {
            data.WriteStream(GetStream());
        }
        public String2 Receive()
        {
            return String2.ReadStream(GetStream(), Encoding.UTF8, String2.CRLF + String2.CRLF);
        }
        public String2 Receive(int length)
        {
            return String2.ReadStream(GetStream(), Encoding.UTF8, length);
        }
        public void Dispose()
        {
            Close();
            if (server != null)
            {
                server.Remove(this);
            }
        }
    }
}
