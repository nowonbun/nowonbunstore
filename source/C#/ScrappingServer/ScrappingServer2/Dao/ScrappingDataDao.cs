using System;
using ScrappingORMCore;
using System.Reflection;
using ScrappingServer.Entity;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Config;

namespace ScrappingServer.Dao
{
    /// <summary>
    /// tbl_스크래핑Data
    /// </summary>
    class ScrappingDataDao : DefaultDao<ScrappingData>
    {
        private ILog logger = LogManager.GetLogger(typeof(ScrappingDataDao));
        /// <summary>
        /// It delete record of table
        /// </summary>
        public void Delete(String idx)
        {
            logger.Info("The function of Delete is starting.");
            Type clsTp = typeof(ScrappingData);
            Table table = clsTp.GetCustomAttribute(typeof(Table)) as Table;
            String sql = "Delete " + table.TableName + " Where idx = '" + idx + "'";
            logger.Info(sql);
            base.Execute(sql);
        }
        /// <summary>
        /// It get number of apply from database.
        /// </summary>
        public List<long> GetIndex(String ApplyNum,String sitecode)
        {
            logger.Info("The function of GetIndex is starting.");
            ScrappingData entity = new ScrappingData();
            entity.ApplyNum = Int64.Parse(ApplyNum);
            entity.SiteCode = sitecode;
            List<ScrappingData> list  = base.select(entity);
            logger.Info("The count result : " + list.Count);
            if (list.Count < 1)
            {
                return null;
            }
            return (from t in list select t.Idx).ToList();
        }
    }
}
