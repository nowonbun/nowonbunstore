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
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.KeyCode).Append("||");
                sb.Append(item.KeyIndex).Append("||");
                sb.Append(item.Data).Append("||");
                sb.Append(item.CreateDate).AppendLine();
            }
            String filepath = CreateCsv(sb.ToString());
            return ExcuteBulk("ScrapingCommonData", filepath);
        }
    }
}
