using System;

namespace WorkServer
{
    public interface IServer : IDisposable
    {
        int Port { get; set; }

        int Timeout { get; set; }

        void ServerStart();

        event Action<IClient> Acception;
    }
}
