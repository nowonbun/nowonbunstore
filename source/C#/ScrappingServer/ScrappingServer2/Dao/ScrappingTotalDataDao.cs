using System;
using ScrappingORMCore;
using System.Reflection;
using ScrappingServer.Entity;

namespace ScrappingServer.Dao
{
    class ScrappingTotalDataDao : DefaultDao<ScrappingTotalData>
    {
        public void Delete(String ApplyNum, String sidecode)
        {
            Type clsTp = typeof(ScrappingTotalData);
            Table table = clsTp.GetCustomAttribute(typeof(Table)) as Table;
            base.Execute("Delete " + table.TableName + " Where 신청번호 = '" + ApplyNum + "' and 사이트코드 = '" + sidecode + "'");
        }
    }
}
