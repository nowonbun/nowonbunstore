using System;
using System.Windows.Forms;
using WebScraping.Scraper.Impl;
using WebScraping.Library.Log;
using WebScraping.Scraper.Other;
using WebScraping.Dao.Common;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Entity;

namespace WebScraping.Scraper
{
    class Scraper
    {
        [STAThread]
        static void Main(string[] args)
        {
            LoggerBuilder.Init("d:\\log\\log4net.xml").Set("Server").Info("Client Program Start");
            if (args.Length != 2)
            {
                throw new ScraperException("Parameter Length Error " + args.Length);
            }
            FactoryDao.CreateInstance("Server=only1.iptime.org;Port=3306;Database=scrap;Uid=nowonbun;Pwd=1234;");
            IScrapingStatusDao dao = FactoryDao.GetInstance().GetDao("WebScraping.Dao.Dao.Impl.ScrapingStatusDao") as IScrapingStatusDao;
            ScrapingStatus entity = dao.GetEntity(args[0]);
            if (entity == null)
            {
                throw new ScraperException("Nothing entity " + args[0]);
            }
            /*if(entity.Status != "0")
            {
                throw new ScraperException("entity status" + args[0]);
            }*/
            entity.Status = "1";
            dao.Update(entity);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScraperContext(args[0], args[1]));
        }
    }
}
