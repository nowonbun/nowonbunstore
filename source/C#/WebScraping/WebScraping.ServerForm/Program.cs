using System;
using System.Windows.Forms;
using WebScraping.Library.Log;
using WebScraping.Dao.Common;
using WebScraping.Library.Config;

namespace WebScraping.ServerForm
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            LoggerBuilder.Init(ConfigSystem.ReadConfig("Config", "Log", "Path")).Set("Server").Info("Server Program Start");
            FactoryDao.CreateInstance(ConfigSystem.ReadConfig("Config", "DB", "Connection"), ConfigSystem.ReadConfig("Config", "Log", "Path"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
