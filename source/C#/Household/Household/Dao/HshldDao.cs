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
    public class HshldDao : AbstractDao<Hshld>
    {
        public void InsertToInfo(String groupId, String id, String cd, String tp, String dt, String cntxt, String prc)
        {
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO HSHLD (GRPD, USRD, CD, TP, DT, CNTXT, PRC, PDT)");
            query.Append("VALUES(@GRPD, @USRD, @CD, @TP, @DT, @CNTXT, @PRC, now())");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@GRPD", groupId));
            param.Add(CreateParameter("@USRD", id));
            param.Add(CreateParameter("@CD", cd));
            param.Add(CreateParameter("@TP", tp));
            param.Add(CreateParameter("@DT", dt));
            param.Add(CreateParameter("@CNTXT", cntxt));
            param.Add(CreateParameter("@PRC", prc));
            base.ExecuteNotResult(query.ToString(), param.ToArray());
        }
        public void UpdateToInfo(String idx, String cd, String tp, String dt, String cntxt, String prc)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE HSHLD ");
            query.Append(" SET ");
            query.Append ("CD = @CD, ");
            query.Append(" TP = @TP, ");
            query.Append(" DT = @DT, ");
            query.Append(" CNTXT = @CNTXT, ");
            query.Append(" PRC = @PRC, ");
            query.Append(" PDT = now() ");
            query.Append(" WHERE NDX = @NDX ");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@NDX", idx));
            param.Add(CreateParameter("@CD", cd));
            param.Add(CreateParameter("@TP", tp));
            param.Add(CreateParameter("@DT", dt));
            param.Add(CreateParameter("@CNTXT", cntxt));
            param.Add(CreateParameter("@PRC", prc));
            base.ExecuteNotResult(query.ToString(), param.ToArray());
        }
        public void DeleteToInfo(String idx)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" DELETE FROM HSHLD ");
            query.Append(" WHERE NDX = @NDX ");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@NDX", idx));
            base.ExecuteNotResult(query.ToString(), param.ToArray());
        }

        public IList<Hshld> SelectToInfoByDate(String groupId, DateTime date)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT NDX,GRPD,USRD,CD,TP,DT,CNTXT,PRC,PDT FROM HSHLD ");
            query.Append(" WHERE GRPD = @GRPD");
            query.Append(" AND DT >= @STDT ");
            query.Append(" AND DT < @NDDT ");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@GRPD", groupId));
            param.Add(CreateParameter("@STDT", date.ToString("yyyy-MM-01")));
            param.Add(CreateParameter("@NDDT", date.AddMonths(1).ToString("yyyy-MM-01")));
            return base.Select(query.ToString(), param.ToArray());
        }

        public Decimal SelectSumAccountTotal(String groupId,DateTime enddate)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT  ");
            query.Append(" SUM_HOUSEHOLDPRICE(@GRPD,'011','20100101',@ENDDATE) ");
            query.Append(" - ");
            query.Append(" SUM_HOUSEHOLDPRICE(@GRPD,'012','20100101',@ENDDATE) ");
            query.Append(" AS PRC ");
            query.Append(" FROM DUAL ");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@GRPD", groupId));
            param.Add(CreateParameter("@ENDDATE", enddate.ToString("yyyyMM01")));
            IList<Hshld> ret = base.Select(query.ToString(), param.ToArray());
            if (ret.Count < 1)
            {
                return Decimal.Zero;
            }
            return ret[0].Prc;
        }
        public IList<Hshld> SelectToCreditByDate(String groupId, DateTime date)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT NDX,GRPD,USRD,CD,TP,DT,CNTXT,PRC,PDT FROM HSHLD ");
            query.Append(" WHERE GRPD = @GRPD");
            query.Append(" AND DT >= @STDT ");
            query.Append(" AND DT < @NDDT ");
            //Don't search the credit.
            query.Append(" AND CD = '020' ");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@GRPD", groupId));
            param.Add(CreateParameter("@STDT", date.ToString("yyyy-MM-01")));
            param.Add(CreateParameter("@NDDT", date.AddMonths(1).ToString("yyyy-MM-01")));
            return base.Select(query.ToString(), param.ToArray());
        }

        public Hshld SelectByIdx(String groupId, String idx)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT NDX,GRPD,USRD,CD,TP,DT,CNTXT,PRC,PDT FROM HSHLD ");
            query.Append(" WHERE GRPD = @GRPD");
            query.Append(" AND NDX = @NDX");
            IList<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(CreateParameter("@GRPD", groupId));
            param.Add(CreateParameter("@NDX", idx));
            IList<Hshld> ret = base.Select(query.ToString(), param.ToArray());
            if (ret.Count < 1)
            {
                return null;
            }
            return ret[0];
        }
    }
}