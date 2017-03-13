using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkServer;
using System.Threading.Tasks;

namespace WorkSocketServer
{
    public interface WorkSocket
    {
        Client SocketClient { get; }

        void Initialize(String2 key);

        void Send(int opcode);

        void Send(int opcode, String2 data);
    }
}
