using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.HshldLogDao")]
    public interface IHshldLogDao : IDao
    {
        void InsertToLog(Hshld val);
    }
}