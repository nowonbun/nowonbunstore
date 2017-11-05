using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;
using System.Data;

namespace LogisticsSystem.Dao
{
    public class OrderTableDao : AbstractDao<OrderTable>
    {
        /// <summary>
        /// 발주번호 생성
        /// </summary>
        public String CreateCode()
        {
            String query = " insert into codeCreater select Cast(isnull(Max(codebuffer)+1,1) as Decimal) as code,4 from codeCreater where type=4";
            Delete(query);
            query = " select Cast(Max(codebuffer)as Decimal) as code from codeCreater where type=4 ";
            DataTable dt = SelectDataTable(query);
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            Decimal code = (Decimal)dt.Rows[0][0];
            return "BA-" + code.ToString("0000000000");
        }
        public int InsertOrder(OrderTable entity)
        {
            return Insert(entity, "tbl_Orderlist");
        }
        /// <summary>
        /// 발주서 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public OrderTable SelectOrderTable(Int64 idx, String companycode)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", companycode);
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_Orderlist where idx = @idx and companycode=@companycode");
            IList<OrderTable> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        public OrderTable SelectOrderTable(String orderNumbur)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_Orderlist where ordernumber = @ordernumber");
            ParameterInit();
            ParameterAdd("ordernumber", orderNumbur);
            IList<OrderTable> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 등록처리
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <returns></returns>

        public int Approve(Int64 idx, int nStateType, String companycode)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("state", nStateType);
            ParameterAdd("companycode", companycode);

            StringBuilder query = new StringBuilder();
            query.Append(" Update tbl_Orderlist ");
            query.Append(" set state = @state ");
            query.Append(" where idx = @idx ");
            query.Append(" and companycode = @companycode ");
            return base.Update(query.ToString(), GetParameter());
        }
        /// <summary>
        /// 발주서 전체 검색 수
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int CountOrder(int orderType, String companycode, bool state/*전체 또는 STATE = 0 경우*/ )
        {
            StringBuilder sb = new StringBuilder();
            ParameterInit();
            ParameterAdd("orderType", orderType);
            ParameterAdd("companycode", companycode);
            if (state)
            {
                ParameterAdd("state", Define.STATE_NORMAL);
            }
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_Orderlist ");
            sb.Append(" WHERE ");
            if (state)
            {
                sb.Append(" state = @state and ");
            }
            sb.Append(" orderType = @orderType and companycode=@companycode ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 발주서 전체 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public IList<OrderTable> SelectOrder(int pageLimit, int page, int orderType, String companycode, bool state/*전체 또는 STATE = 0 경우*/ )
        {
            StringBuilder sb = new StringBuilder();
            ParameterInit();
            ParameterAdd("orderType", orderType);
            ParameterAdd("companycode", companycode);
            if (state)
            {
                ParameterAdd("state", Define.STATE_NORMAL);
            }
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_Orderlist ");
            sb.Append(" WHERE ");
            if (state)
            {
                sb.Append(" state = @state and ");
            }
            sb.Append(" orderType = @orderType and companycode=@companycode ");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_Orderlist WHERE ");
            if (state)
            {
                sb.Append(" state = @state and ");
            }
            sb.Append(" orderType = @orderType and companycode=@companycode order by idx desc) ");
            sb.Append(" order by idx desc");

            return Select(sb.ToString(), GetParameter());
        }
    }
}