using PTM.Httpd.Util;

namespace PTM.Httpd
{
    public class WebSocketNode
    {
        public Opcode OPCode { get; set; }
        public String2 Message { get; set; }
        public bool IsBroadCast { get; set; }
        public WebSocketNode()
        {
            IsBroadCast = false;
        }
    }
}
