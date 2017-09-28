using System;
using System.Windows.Forms;
using WebScraping.Scraper.Impl;
using WebScraping.Library.Log;
using WebScraping.Scraper.Other;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScraperContext(args[0], args[1]));
        }
    }
}
