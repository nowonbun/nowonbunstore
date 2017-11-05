using System;
using System.Collections.Generic;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class BillDao : AbstractDao<Bill>
    {
        public void InsertBill(Bill entity)
        {
            Insert(entity, "tbl_bill");
        }
        public Int64 GetScopeIndentity()
        {
            return ScopeIndentity("tbl_bill");
        }
        public Bill SelectByIdx(Int64 idx, String companycode)
        {
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", companycode);
            query.Append("SELECT * FROM tbl_Bill where idx = @idx and companycode=@companycode order by idx desc");
            IList<Bill> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 정산서 검색수
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int GetBillCountByCompanyCode(String companycode)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("companycode", companycode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_bill ");
            sb.Append(" WHERE state = @state ");
            sb.Append(" AND companycode = @companycode");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 정산서 검색
        /// </summary>
        /// <param name="pageLimit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IList<Bill> SelectBill(int pageLimit, int page, String companycode)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("companycode", companycode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_bill ");
            sb.Append(" WHERE state = @state and companycode=@companycode ");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_bill WHERE state = @state and companycode=@companycode order by idx desc) ");
            sb.Append(" order by idx desc");
            return Select(sb.ToString(), GetParameter());
        }
    }
}