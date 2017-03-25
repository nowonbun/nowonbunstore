using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.SysDtDao")]
    public interface ISysDtDao : IDao
    {
        IList<SysDt> SelectAll();
    }
}