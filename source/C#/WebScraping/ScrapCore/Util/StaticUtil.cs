using System;
using System.Runtime.InteropServices;

namespace ScrapCore
{
    class StaticUtil
    {
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr IpBuffer, int IpdwBufferLength);

        //This is to Initialize of web session.
        public static void InternetSetOption(IntPtr hInternet){
            InternetSetOption(hInternet, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }
        
    }
}
