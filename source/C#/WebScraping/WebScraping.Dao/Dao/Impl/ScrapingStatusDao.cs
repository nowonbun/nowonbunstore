using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;
using WebScraping.Dao.Dao;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingStatusDao : Dao<ScrapingStatus>, IScrapingStatusDao
    {
        public IList<ScrapingStatus> Select()
        {
            return base.SelectByEntity(null);
        }

        public ScrapingStatus GetEntity(String keycode)
        {
            ScrapingStatus where = new ScrapingStatus();
            where.KeyCode = keycode;
            IList<ScrapingStatus> list = base.SelectByEntity(where);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
        public int Update(ScrapingStatus entity)
        {
            return base.UpdateByEntity(entity);
        }
        public int Insert(ScrapingStatus entity)
        {
            return base.InsertByEntity(entity);
        }
    }
}
