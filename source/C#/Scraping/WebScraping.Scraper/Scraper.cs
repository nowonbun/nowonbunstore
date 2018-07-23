using System;
using System.Windows.Forms;
using WebScraping.Scraper.Impl;
using WebScraping.Library.Log;
using WebScraping.Scraper.Other;
using WebScraping.Library.Config;
using Newtonsoft.Json;
using System.Text;
using WebScraping.Scraper.Common;

namespace WebScraping.Scraper
{
    class Scraper
    {
        [STAThread]
        static void Main(string[] args)
        {
            Logger logger = null;
            try
            {
                String arg = null;
                if (Debug.IsDebug())
                {
                    arg = Debug.GetDebugParam();
                }
                else
                {
                    if (args.Length != 1)
                    {
                        throw new ScraperException("Parameter Length Error " + args.Length);
                    }
                    byte[] buffer = Convert.FromBase64String(args[0]);
                    arg = Encoding.UTF8.GetString(buffer);
                }
                Parameter param = JsonConvert.DeserializeObject<Parameter>(arg);
                logger = LoggerBuilder.Init(LogTemplate.GetLogTemp(ConfigSystem.ReadConfig("Config", "Log", "WritePath") + "\\" + param.Key + ".log")).Set("Server").Info("Client Program Start");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (Debug.IsDebug())
                {
                    Application.Run(new ScraperForm(param));
                }
                else
                {
                    Application.Run(new ScraperContext(param));
                }
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Error(e.ToString());
                }
                Exit();
            }
        }
        public static void Exit()
        {
            LoggerBuilder.Init().Set("Server").Info("Client Program Exit");
            BrokerSender.Abort();
            Application.Exit();
        }
    }
}
