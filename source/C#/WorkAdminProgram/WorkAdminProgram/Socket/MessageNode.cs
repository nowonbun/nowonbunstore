using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkAdminProgram
{
    class MessageNode
    {
        public MessageType MessageType { get; set; }
        public String Message { get; set; }
        public String WorkTitle { get; set; }
        public IList<String> Files { get; set; }
    }
}
