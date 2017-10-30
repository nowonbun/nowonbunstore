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
            return base.SelectAll();
        }
        public int Insert(ScrapingPackageData entity)
        {
            return base.InsertByEntity(entity);
        }
        public int Update(ScrapingPackageData entity)
        {
            return base.UpdateByEntity(entity);
        }
        public int Delete(ScrapingPackageData entity)
        {
            return base.DeleteByEntity(entity);
        }
        public int InsertList(IList<ScrapingPackageData> list)
        {
            int count = 0;
            String query = CreateInsertQuery();
            base.Transaction(() =>
            {
                foreach (var item in list)
                {
                    count += base.ExcuteNonReader(query, SetParameter(item));
                }
            });
            return count;
        }
    }
}
