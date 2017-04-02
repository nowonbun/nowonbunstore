using System;

namespace WorkServer
{
    public static class Util
    {
        public static byte[] Reverse(byte[] data)
        {
            Array.Reverse(data);
            return data;
        }
    }
}
