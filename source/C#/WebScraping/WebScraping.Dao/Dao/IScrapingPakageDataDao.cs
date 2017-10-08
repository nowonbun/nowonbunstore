using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    [ImplementDao("WebScraping.Dao.Dao.Impl.ScrapingPakageDataDao")]
    public interface IScrapingPakageDataDao
    {
        IList<ScrapingPackageData> Select();

        int Insert(ScrapingPackageData entity);
    }
}
