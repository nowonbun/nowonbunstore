using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WebScraping.WebServer.Impl
{
    class ClientSocket : TcpClient, IClientSocket
    {
        public static implicit operator ClientSocket(Socket s)
        {
            ClientSocket r = new ClientSocket();
            r.Client = s;
            return r;
        }
        public void Run()
        {
            byte[] buffer = new byte[4096];
            using (Stream stream = GetStream())
            {
                stream.Read(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.Default.GetString(TrimByte(buffer)));
            }
        }
        private byte[] TrimByte(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    byte[] buffer = new byte[i];
                    Array.Copy(data, buffer, i);
                    return buffer;
                }
            }
            return data;
        }
    }
}
