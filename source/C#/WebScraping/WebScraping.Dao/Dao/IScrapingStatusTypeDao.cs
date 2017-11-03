using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    public interface IScrapingStatusTypeDao
    {
        IList<ScrapingStatusType> Select();

        int Insert(ScrapingStatusType entity);

        int Update(ScrapingStatusType entity);

        int Delete(ScrapingStatusType entity);
    }
}
