using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkServer
{
    public enum Opcode : int
    {
        MESSAGE = 1,
        BINARY = 2,
        EXIT = 8,
        PING = 9,
        PONG = 10
    }
}
