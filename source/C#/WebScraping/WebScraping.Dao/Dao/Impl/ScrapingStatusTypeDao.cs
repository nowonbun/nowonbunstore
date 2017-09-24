using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingStatusTypeDao : Dao<ScrapingStatusType>, IScrapingStatusTypeDao
    {
        public IList<ScrapingStatusType> Select()
        {
            return base.SelectByEntity(null);
        }
    }
}
