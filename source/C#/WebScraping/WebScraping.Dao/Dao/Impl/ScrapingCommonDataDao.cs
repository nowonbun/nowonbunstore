using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;
using System.Text;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Linq;
using WebScraping.Dao.Interface;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingCommonDataDao : Dao<ScrapingCommonData>, IScrapingCommonDataDao
    {
        public IList<ScrapingCommonData> Select()
        {
            return base.SelectAll();
        }
        public int Insert(ScrapingCommonData entity)
        {
            return base.InsertByEntity(entity);
        }
        public int Update(ScrapingCommonData entity)
        {
            return base.UpdateByEntity(entity);
        }
        public int Delete(ScrapingCommonData entity)
        {
            return base.DeleteByEntity(entity);
        }
        public int InsertList(IList<ScrapingCommonData> list)
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
