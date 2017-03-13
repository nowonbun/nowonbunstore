using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSocketServer
{
    public enum MessageType : int
    {
        MESSAGE = 1,
        FILELIST = 2,
        WORKLIST = 3,
        WORKTEMP = 4,
    }

    public enum FileMessageType : byte
    {
        FileOpen = 0x0A,
        FileWrite = 0x0B,
        FileSearch = 0x0C,
        FileListNotice = 0x0D,
    }

    public enum WorkType : byte
    {
        WorkListNotice = 0x09,
        WorkSearch = 0x08,
        WorkTemp = 0x07
    }

    public enum OPCODE : int
    {
        MESSAGE = 1,
        BINARY = 2,
        EXIT = 8,
        PING = 9,
        PONG = 10
    }
}
