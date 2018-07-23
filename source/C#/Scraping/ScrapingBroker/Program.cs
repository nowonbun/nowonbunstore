using System;
using System.Windows.Forms;
using WebScraping.Library.Log;
using WebScraping.Library.Config;
using System.Threading;

namespace WebScraping.Broker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string mtxName = "WebScraping";
            Mutex mtx = new Mutex(true, mtxName);
            TimeSpan tsWait = new TimeSpan(0, 0, 1);
            bool success = mtx.WaitOne(tsWait);
            if (!success)
            {
                MessageBox.Show("이미실행중입니다.");
                return;
            }
            LoggerBuilder.Init(LogTemplate.GetLogTemp(ConfigSystem.ReadConfig("Config", "Log", "WritePath") + "\\Broker.log"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
