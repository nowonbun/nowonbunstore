using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingStatusTypeDao : Dao<ScrapingStatusType>, IScrapingStatusTypeDao
    {
        public IList<ScrapingStatusType> Select()
        {
            return base.SelectAll();
        }
        public int Insert(ScrapingStatusType entity)
        {
            return base.InsertByEntity(entity);
        }
        public int Update(ScrapingStatusType entity)
        {
            return base.UpdateByEntity(entity);
        }
        public int Delete(ScrapingStatusType entity)
        {
            return base.DeleteByEntity(entity);
        }
    }
}
