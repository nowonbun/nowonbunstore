using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingPakageDataDao : Dao<ScrapingPakageData>, IScrapingPakageDataDao
    {
        public IList<ScrapingPakageData> Select()
        {
            return base.SelectByEntity(null);
        }
    }
}
