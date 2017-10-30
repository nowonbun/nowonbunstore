using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;
using MySql.Data.MySqlClient;
using WebScraping.Dao.Dao;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingStatusDao : Dao<ScrapingStatus>, IScrapingStatusDao
    {
        public IList<ScrapingStatus> Select()
        {
            return base.SelectAll();
        }

        public ScrapingStatus GetEntity(String keycode)
        {
            String query = "SELECT * FROM ScrapingStatus WHERE KEYCODE = @KEYCODE";
            return base.Transaction(() =>
            {
                ScrapingStatus ret = null;
                base.ExcuteReader(query, new List<MySqlParameter>() { CreateParameter("@KEYCODE", keycode, MySqlDbType.VarChar) }, (dr) =>
                {
                    ret = SetClass<ScrapingStatus>(dr);
                });
                return ret;
            });
        }
        public int Insert(ScrapingStatus entity)
        {
            return base.InsertByEntity(entity);
        }
        public int Update(ScrapingStatus entity)
        {
            return base.UpdateByEntity(entity);
        }
        public int Delete(ScrapingStatus entity)
        {
            return base.DeleteByEntity(entity);
        }
    }
}
