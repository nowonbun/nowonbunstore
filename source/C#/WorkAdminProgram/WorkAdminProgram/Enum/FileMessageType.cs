using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkAdminProgram
{
    public enum FileMessageType : byte
    {
        FileOpen = 0x0A,
        FileWrite = 0x0B,
        FileSearch = 0x0C,
        FileListNotice = 0x0D,
    }
}
