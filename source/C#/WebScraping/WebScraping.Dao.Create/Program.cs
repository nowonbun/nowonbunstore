using System;
using WebScraping.Library.Config;

namespace WebScraping.Dao.Create
{
    class Program
    {
        static void Main(string[] args)
        {
            /*String str = ConfigSystem.ReadConfig("Config", "DB", "Connection");
            AbstractDaoCreate run = new CreateDaoByDatabase(str);

            run.Run("c:\\work\\test\\IDao");

            run = new CreateEntityByDatabase(str);
            run.Run("c:\\work\\test\\Entity");*/

            new CreateEntityByExcel("D:\\Temp\\template1.xls","d:\\temp", "ReturnRequest", "WebScraping.Scraper.Flow.Auction");

            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
    }
}
