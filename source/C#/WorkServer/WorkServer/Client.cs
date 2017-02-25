using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using log4net;
using System.IO;

namespace WorkServer
{
    class Client : TcpClient
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(Client));
        private Stream stream;
        public Client(Socket sock)
        {
            this.Client = sock;
            stream = GetStream();
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
            return String2.ReadStream(stream, Encoding.UTF8, String2.CRLF + String2.CRLF);
        }
        public String2 Receive(int length)
        {
            return String2.ReadStream(stream, Encoding.UTF8, length);
        }
        public void SetTimeout(int time)
        {
            stream.ReadTimeout = time;
        }
        public void Dispose()
        {
            Close();
        }
    }
}
