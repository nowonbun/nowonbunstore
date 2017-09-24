using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    [ImplementDao("WebScraping.Dao.Dao.Impl.ScrapingCommonDataDao")]
    public interface IScrapingCommonDataDao
    {
        IList<ScrapingCommonData> Select();
    }
}
