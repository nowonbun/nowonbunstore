using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.HshldDao")]
    public interface IHshldDao : IDao
    {
        void InsertToInfo(String groupId, String id, String cd, String tp, String dt, String cntxt, String prc);
        void UpdateToInfo(String idx, String cd, String tp, String dt, String cntxt, String prc);
        void DeleteToInfo(String idx);
        IList<Hshld> SelectToInfoByDate(String groupId, DateTime date);
        Decimal SelectSumAccountTotal(String groupId, DateTime enddate);
        IList<Hshld> SelectToCreditByDate(String groupId, DateTime date);
        Hshld SelectByIdx(String groupId, String idx);
    }
}