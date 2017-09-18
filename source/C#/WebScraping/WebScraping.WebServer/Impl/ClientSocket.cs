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
        private String[] _header;
        private String _response;
        private Commander commender;

        public String[] Header
        {
            get { return this._header; }
        }
        public String Path { get; set; }

        public static implicit operator ClientSocket(Socket s)
        {
            ClientSocket r = new ClientSocket();
            r.commender = new Commander(r);
            r.Client = s;
            return r;
        }
        public void Run()
        {
            byte[] buffer = new byte[4096];
            using (Stream stream = GetStream())
            {
                stream.Read(buffer, 0, buffer.Length);
                String msg = Encoding.Default.GetString(TrimByte(buffer));
                //Log header
                try
                {
                    _header = GetHeader(msg);
                    commender.Run(_header[1], stream);
                    //byte[] rep = CreateResponse(200, "OK");
                    //stream.Write(rep, 0, rep.Length);
                   
                }
                catch (Exception e)
                {
                    byte[] rep = CreateResponse(400, "Bad Request");
                    stream.Write(rep, 0, rep.Length);
                }
            }
        }
        private byte[] CreateResponse(int code, String msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HTTP/1.1 ").Append(code).Append(" ").Append(msg).AppendLine();
            sb.AppendLine("Connection: close").AppendLine();
            String ret = sb.ToString();
            return Encoding.Default.GetBytes(ret);
        }
        private String[] GetHeader(String header)
        {
            if (header.Length < 0)
            {
                throw new Exception();
            }
            int pos = header.IndexOf("\r\n");
            if (pos < 0)
            {
                throw new Exception();
            }
            header = header.Substring(0, pos);
            return header.Split(' ');
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
