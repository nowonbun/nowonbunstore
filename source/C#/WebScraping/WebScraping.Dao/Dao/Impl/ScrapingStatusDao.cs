using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingStatusDao : Dao<ScrapingStatus>, IScrapingStatusDao
    {
        public IList<ScrapingStatus> Select()
        {
            return base.SelectByEntity(null);
        }
    }
}
