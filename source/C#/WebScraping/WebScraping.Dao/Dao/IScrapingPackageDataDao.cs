using System;
using System.Collections.Generic;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao
{
    public interface IScrapingPackageDataDao
    {
        IList<ScrapingPackageData> Select();

        int Insert(ScrapingPackageData entity);

        int Update(ScrapingPackageData entity);

        int Delete(ScrapingPackageData entity);

        int InsertList(IList<ScrapingPackageData> list);
    }
}
