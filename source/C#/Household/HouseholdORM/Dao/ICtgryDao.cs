using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.CtgryDao")]
    public interface ICtgryDao : IDao
    {
        IList<Ctgry> SelectAll();
    }
}