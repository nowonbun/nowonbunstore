using System;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.CnctLgDao")]
    public interface ICnctLgDao : IDao
    {
        void InsertToSignin(String groupId, String id);
    }
}
