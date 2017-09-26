using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Impl;

namespace WebScraping.Scraper
{
    class Scraper
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                //error;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScraperContext(args[0], args[1]));
        }
    }
}
