using System;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace PTM.StartConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string mtxName = "PrivateTaskManager";
            Mutex mtx = new Mutex(true, mtxName);

            TimeSpan tsWait = new TimeSpan(0, 0, 1);
            bool success = mtx.WaitOne(tsWait);

            if (!success)
            {
                String port = ConfigSystem.GetSettingPort();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:" + port + "/Start");
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine(response.StatusCode);
                }
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainContext());
        }
    }
}
