using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingCommonDataDao : Dao<ScrapingCommonData>, IScrapingCommonDataDao
    {
        public IList<ScrapingCommonData> Select()
        {
            return base.SelectByEntity(null);
        }
    }
}
