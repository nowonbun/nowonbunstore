using System;
using System.Collections.Generic;
using WebScraping.Dao.Common;
using WebScraping.Dao.Entity;
using System.Text;

namespace WebScraping.Dao.Dao.Impl
{
    class ScrapingPackageDataDao : Dao<ScrapingPackageData>, IScrapingPackageDataDao
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
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.KeyCode).Append("||");
                sb.Append(item.KeyIndex).Append("||");
                sb.Append(item.Separation).Append("||");
                sb.Append(item.Data).Append("||");
                sb.Append(item.CreateDate).AppendLine();
            }
            String filepath = CreateCsv(sb.ToString());
            return ExcuteBulk("ScrapingPackageData", filepath);
        }
    }
}
