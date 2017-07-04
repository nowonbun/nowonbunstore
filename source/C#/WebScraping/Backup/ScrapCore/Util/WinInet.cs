using System;
using System.Runtime.InteropServices;

namespace ScrapCore
{
    class WinInet
    {
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr IpBuffer, int IpdwBufferLength);
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        public static bool InternetSetOption(IntPtr hInternet)
        {
            return InternetSetOption(hInternet, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }
    }
}
