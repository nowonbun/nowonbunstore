using System;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HouseholdORM
{
    class SysDtDao : Dao<SysDt>, ISysDtDao
    {
        public IList<SysDt> SelectAll()
        {
            return base.SelectByEntity(null);
        }
    }
}