using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebScraping.Library.Log;
using WebScraping.Dao.Common;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Entity;

namespace WebScraping.ServerForm
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoggerBuilder.Init("d:\\log\\log4net.xml").Set("Server").Info("Server Program Start");
            FactoryDao.CreateInstance("Server=only1.iptime.org;Port=3306;Database=scrap;Uid=nowonbun;Pwd=1234;");
            //Test test = new Test();
            //test.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
             
        }
        /*class Test : Allocation
        {
            [ResourceDao]
            private IScrapingStatusTypeDao testdao;

            public void Run()
            {
                IList<ScrapingStatusType> list =  testdao.Select();
                foreach (var l in list)
                {
                    Console.WriteLine(l.StatusName);
                }
            }
        }*/
    }
}
