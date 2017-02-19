using System;
using Household.Models.Entity;
using HouseholdORM;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;

namespace Household.Dao
{
    public class HshldLogDao : AbstractDao<HshldLog>
    {
        public void InsertToLog(Hshld val)
        {
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO HSHLD_LOG (NDX2, GRPD, USRD, CD, TP, DT, CNTXT, PRC, PDT, CDT)");
            query.Append("VALUES(@NDX, @GRPD, @USRD, @CD, @TP, @DT, @CNTXT, @PRC, @PDT, now())");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@NDX", val.Ndx));
            param.Add(CreateParameter("@GRPD", val.Grpd));
            param.Add(CreateParameter("@USRD", val.Usrd));
            param.Add(CreateParameter("@CD", val.Cd));
            param.Add(CreateParameter("@TP", val.Tp));
            param.Add(CreateParameter("@DT", val.Dt));
            param.Add(CreateParameter("@CNTXT", val.Cntxt));
            param.Add(CreateParameter("@PRC", val.Prc));
            param.Add(CreateParameter("@PDT", val.Pdt));
            base.ExecuteNotResult(query.ToString(), param.ToArray());
        }
    }
}