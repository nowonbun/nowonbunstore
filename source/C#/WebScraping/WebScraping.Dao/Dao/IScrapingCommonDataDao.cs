using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    public interface IScrapingCommonDataDao
    {
        IList<ScrapingCommonData> Select();

        int Insert(ScrapingCommonData entity);

        int Update(ScrapingCommonData entity);

        int Delete(ScrapingCommonData entity);

        int InsertList(IList<ScrapingCommonData> list);

        void Test();
    }
}
