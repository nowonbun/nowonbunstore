using System;
using System.Windows.Forms;
using WebScraping.Scraper.Impl;
using WebScraping.Library.Log;
using WebScraping.Scraper.Other;
using WebScraping.Dao.Common;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Entity;
using WebScraping.Library.Config;

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
                if (Debug.IsDebug())
                {
                    args = new String[] { Debug.GetDebugKey(), Debug.GetDebugParam() };
                }
                if (args.Length != 2)
                {
                    throw new ScraperException("Parameter Length Error " + args.Length);
                }
                logger = LoggerBuilder.Init(LogTemplate.GetLogTemp(ConfigSystem.ReadConfig("Config", "Log", "WritePath") + "\\" + args[0] + ".log")).Set("Server").Info("Client Program Start");
                logger.Debug("arg[0] : " + args[0]);
                logger.Debug("arg[1] : " + args[1]);
                FactoryDao.CreateInstance(ConfigSystem.ReadConfig("Config", "DB", "Connection"), ConfigSystem.ReadConfig("Config", "Temp", "Path"));
                IScrapingStatusDao dao = FactoryDao.GetInstance().GetDao<IScrapingStatusDao>();
                if (!Debug.IsDebug())
                {
                    ScrapingStatus entity = dao.GetEntity(args[0]);

                    if (entity == null)
                    {
                        throw new ScraperException("Nothing entity " + args[0]);
                    }

                    if (entity.Status != "0")
                    {
                        throw new ScraperException("entity status" + args[0]);
                    }
                    entity.Status = "1";
                    dao.Update(entity);
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ScraperContext(args[0], args[1]));
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
            Application.Exit();
        }
    }
}
