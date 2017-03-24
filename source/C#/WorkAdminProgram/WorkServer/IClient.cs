using System;
using System.Net;

namespace WorkServer
{
    public interface IClient : IDisposable
    {
        bool Connected { get; }

        EndPoint RemoteEndPoint { get; }

        void SetTimeout(int time);

        void Send(byte[] data);

        String2 Receive();

        String2 Receive(int length);

        void Close();
    }
}
