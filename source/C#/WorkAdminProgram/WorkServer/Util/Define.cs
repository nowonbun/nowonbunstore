using System.Text;

namespace WorkServer
{
    static class Define
    {
        public static Encoding encoding = Encoding.UTF8;
        public static byte[] CRLF = new byte[] { 0x0D, 0x0A, 0x0D, 0x0A };
        public static int BUFFER_SIZE = 4096;
    }
}
