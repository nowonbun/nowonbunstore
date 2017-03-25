using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.GrpNfDao")]
    public interface IGrpNfDao : IDao
    {
        IList<GrpNf> SelectByKey(String grpd);
    }
}