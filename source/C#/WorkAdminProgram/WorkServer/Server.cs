using System;

namespace WorkServer
{
    public interface Server : IDisposable
    {
        int Port { get; set; }
        int Timeout { get; set; }

        void ServerStart();

        event Action<Client> Acception;
    }
}
