using System;
using Household.Models.Entity;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Household.Dao
{
    public class SysDtDao : AbstractDao<SysDt>
    {
        public IList<SysDt> SelectAll()
        {
            return base.SelectByEntity(null);
        }
    }
}