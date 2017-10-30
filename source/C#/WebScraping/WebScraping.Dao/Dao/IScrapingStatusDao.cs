using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    [ImplementDao("WebScraping.Dao.Dao.Impl.ScrapingStatusDao")]
    public interface IScrapingStatusDao
    {
        IList<ScrapingStatus> Select();

        int Insert(ScrapingStatus entity);

        int Update(ScrapingStatus entity);

        int Delete(ScrapingStatus entity);

        ScrapingStatus GetEntity(String keycode);
    }
}
