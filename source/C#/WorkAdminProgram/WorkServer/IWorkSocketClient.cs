using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkServer;
using System.Threading.Tasks;

namespace WorkServer
{
    public interface IWorkSocketClient
    {
        IClient SocketClient { get; }

        Exception LastException { get; }

        void Initialize(String2 key);

        void SetReceiveEvent(Action<IWorkSocketClient, byte, String2> receive);

        void Send(int opcode);

        void Send(int opcode, String2 data);
    }
}
