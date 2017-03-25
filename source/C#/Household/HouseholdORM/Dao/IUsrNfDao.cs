using System;
using System.Collections.Generic;

namespace HouseholdORM
{
    [ImplementDao("HouseholdORM.UsrNfDao")]
    public interface IUsrNfDao : IDao
    {
        UsrNf SelectForSign(String groupId, String id, String pw);
    }
}