using System;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HouseholdORM
{
    class GrpNfDao : Dao<GrpNf>, IGrpNfDao
    {
        public IList<GrpNf> SelectByKey(String grpd)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM GRP_NF WHERE GRPD = @GRPD");
            return base.Select(query.ToString(),CreateParameter("@GRPD",grpd));
        }
    }
}