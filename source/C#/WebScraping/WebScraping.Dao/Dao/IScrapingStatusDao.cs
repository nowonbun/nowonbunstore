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

        ScrapingStatus GetEntity(String keycode);

        int Update(ScrapingStatus entity);

        int Insert(ScrapingStatus entity);
    }
}
