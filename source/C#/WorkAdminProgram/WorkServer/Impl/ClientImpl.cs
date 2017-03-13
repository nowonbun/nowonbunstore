using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WorkServer
{
    class ClientImpl : TcpClient, IDisposable, Client
    {

        private Stream stream;

        public ClientImpl(Socket sock)
        {
            this.Client = sock;
            stream = GetStream();
        }

        public static implicit operator ClientImpl(Socket sock)
        {
            return new ClientImpl(sock);
        }

        public new bool Connected
        {
            get { return base.Connected; }
        }

        public EndPoint RemoteEndPoint
        {
            get { return base.Client.RemoteEndPoint; }
        }

        public void Send(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public String2 Receive()
        {
            String2 buffer = new String2(Define.BUFFER_SIZE);
            String2 retBuffer = new String2(0);
            int revlength = 0;
            while ((revlength = stream.Read(buffer.ToBytes(), 0, buffer.Length)) > 0)
            {
                buffer = buffer.SubString(0, revlength);
                retBuffer += buffer;
                
                if (retBuffer.CheckEnd(Define.CRLF))
                {
                    break;
                }
                buffer = new byte[Define.BUFFER_SIZE];
            }
            return retBuffer;
        }

        public String2 Receive(int length)
        {
            String2 buffer = new String2(length);
            String2 retBuffer = new String2(0);
            int revlength = 0;
            int totallength = 0;
            while ((revlength = stream.Read((byte[])buffer, 0, length)) > 0)
            {
                buffer = buffer.SubString(0, revlength);
                retBuffer += buffer;
                totallength += revlength;
                if (totallength >= length)
                {
                    break;
                }
            }
            return retBuffer;
        }

        public void SetTimeout(int time)
        {
            stream.ReadTimeout = time;
        }

        public new void Close()
        {
            base.Close();
        }
    }
}
