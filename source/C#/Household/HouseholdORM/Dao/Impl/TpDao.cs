using System;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HouseholdORM
{
    class TpDao : Dao<Tp>, ITpDao
    {
        public IList<Tp> SelectAll()
        {
            return base.SelectByEntity(null);
        }
    }
}