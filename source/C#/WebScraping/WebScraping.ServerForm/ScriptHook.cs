using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace WebScraping.ServerForm
{
    class ScriptHook : IDisposable
    {
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int hWnd1, int hWnd2, string lpsz1, string lpsz2);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd1, uint msg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        private Timer timer;

        public ScriptHook()
        {
            timer = new Timer(new TimerCallback(CheckingTimer), null, 0, 100);
        }
        public void Dispose()
        {
            timer.Dispose();
        }
        private void CloseHandle(String titlename)
        {
            int hWnd = (int)FindWindow(null, titlename);
            if (hWnd > 0)
            {
                SendMessage(hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
                SendMessage(hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
        }
        protected void CheckingTimer(object obj)
        {
            //CloseHandle("웹 페이지 메시지");
        }
    }
}
