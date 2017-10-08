using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingPakageDataDao : Dao<ScrapingPackageData>, IScrapingPakageDataDao
    {
        public IList<ScrapingPackageData> Select()
        {
            return base.SelectByEntity(null);
        }
        public int Insert(ScrapingPackageData entity)
        {
            return base.InsertByEntity(entity);
        }
    }
}
