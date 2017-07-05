using System;

namespace AisProjectCore.Dinject.Abstract
{
    public interface ISocket
    {
        void Run();
        void Shutdown();
    }
}
