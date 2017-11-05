using System;
using ScrappingORMCore;
using System.Reflection;
using ScrappingServer.Entity;
using log4net;
using log4net.Config;

namespace ScrappingServer.Dao
{
    /// <summary>
    /// tbl_스크래핑Data_sub
    /// </summary>
    class ScrappingDataSubDao : DefaultDao<ScrappingDataSub>
    {
        private ILog logger = LogManager.GetLogger(typeof(ScrappingDataDao));
        /// <summary>
        /// It delete record of table.
        /// </summary>
        /// <param name="Dataindex"></param>
        public void Delete(String Dataindex)
        {
            Type clsTp = typeof(ScrappingDataSub);
            Table table = clsTp.GetCustomAttribute(typeof(Table)) as Table;
            String sql = "Delete " + table.TableName + " Where Dataidx = '" + Dataindex + "'";
            logger.Info(sql);
            base.Execute(sql);
        }
    }
}
