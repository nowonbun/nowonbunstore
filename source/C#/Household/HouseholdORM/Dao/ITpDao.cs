using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.TpDao")]
    public interface ITpDao : IDao
    {
        IList<Tp> SelectAll();
    }
}