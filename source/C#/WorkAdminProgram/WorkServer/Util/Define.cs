using System;
using System.Text;

namespace WorkServer
{
    static class Define
    {
        public static Encoding encoding = Encoding.UTF8;
        public static byte[] CRLF = new byte[] { 0x0D, 0x0A, 0x0D, 0x0A };
        public static int BUFFER_SIZE = 4096;
        public static String GUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        public static String WEB_SEPARATOR = "/";
        public static String DOWNROAD_PATH = "/download";
    }
}
