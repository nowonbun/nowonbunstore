
namespace WebScraping.WebServer
{
    public class WebSocketNode
    {
        public Opcode OPCode { get; set; }
        public String2 Message { get; set; }
        public bool Broadcast { get; set; }
        public WebSocketNode()
        {
            OPCode = Opcode.MESSAGE;
            Message = new String2(0);
            Broadcast = false;
        }
    }
}
