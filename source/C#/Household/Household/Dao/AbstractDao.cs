using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;

namespace Household.Dao
{
    public abstract class AbstractDao<T> : Dao<T>
    {
        protected override String GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["householdConn"].ConnectionString;
        }
    }
}