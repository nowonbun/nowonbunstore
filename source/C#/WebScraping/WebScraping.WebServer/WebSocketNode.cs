using System;
using System.Text;

namespace WebScraping.WebServer
{
    public class WebSocketNode
    {
        public Opcode OPCode { get; set; }
        public byte[] Message { get; set; }
        public void SetMessage(String message)
        {
            Message = Encoding.UTF8.GetBytes(message);
        }
        public String GetMessage()
        {
            return Encoding.UTF8.GetString(Message);
        }
    }
}
