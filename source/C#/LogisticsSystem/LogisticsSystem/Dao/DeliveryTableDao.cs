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
    public class DeliveryTableDao : AbstractDao<DeliveryTable>
    {
        /// <summary>
        /// 데이터입력
        /// </summary>
        /// <returns></returns>
        public void InsertDelivery(DeliveryTable entity)
        {
            base.Insert(entity, "tbl_Delivery");
        }
        public Int64 GetScopeIndentity()
        {
            return ScopeIndentity("tbl_Delivery");
        }

        /// <summary>
        /// 납품확인서 취득
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public DeliveryTable SelectByIdx(Int64 idx, String companycode)
        {
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", companycode);
            query.Append("SELECT * FROM tbl_Delivery where idx = @idx order by idx desc");
            IList<DeliveryTable> list = base.Select(query.ToString(), GetParameter());
            if(list.Count < 1){
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// 납품확인서 카운트 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <returns></returns>
        public int GetDeliveryCount(String companycode)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("companycode", companycode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_Delivery ");
            sb.Append(" WHERE state = @state ");
            sb.Append(" AND companycode = @companycode ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 남품확인서 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<DeliveryTable> SelectDelivery(int pageLimit, int page, String companycode)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("companycode", companycode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_Delivery ");
            sb.Append(" WHERE state = @state and companycode = @companycode ");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_Delivery WHERE state = @state and companycode = @companycode order by idx desc) ");
            sb.Append(" order by idx desc");

            return Select(sb.ToString(), GetParameter());
        }
    }
}