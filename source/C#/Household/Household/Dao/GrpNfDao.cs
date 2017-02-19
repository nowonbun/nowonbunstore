using System;
using Household.Models.Entity;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Household.Dao
{
    public class GrpNfDao : AbstractDao<GrpNf>
    {
        public IList<GrpNf> SelectByKey(String grpd)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM GRP_NF WHERE GRPD = @GRPD");
            return base.Select(query.ToString(),CreateParameter("@GRPD",grpd));
        }
    }
}