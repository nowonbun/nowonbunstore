using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    [ImplementDao("WebScraping.Dao.Dao.Impl.ScrapingStatusTypeDao")]
    public interface IScrapingStatusTypeDao
    {
        IList<ScrapingStatusType> Select();

        int Insert(ScrapingStatusType entity);

        int Update(ScrapingStatusType entity);

        int Delete(ScrapingStatusType entity);
    }
}
