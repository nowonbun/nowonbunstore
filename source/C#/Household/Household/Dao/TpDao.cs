using System;
using Household.Models.Entity;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Household.Dao
{
    public class TpDao : AbstractDao<Tp>
    {
        public IList<Tp> SelectAll()
        {
            return base.SelectByEntity(null);
        }
    }
}