using System;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HouseholdORM
{
    class CtgryDao : Dao<Ctgry>, ICtgryDao
    {
        public IList<Ctgry> SelectAll()
        {
            return base.SelectByEntity(null);
        }
    }
}