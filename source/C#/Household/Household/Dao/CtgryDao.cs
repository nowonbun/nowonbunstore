using System;
using Household.Models.Entity;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Household.Dao
{
    public class CtgryDao : AbstractDao<Ctgry>
    {
        public IList<Ctgry> SelectAll()
        {
            return base.SelectByEntity(null);
        }
    }
}