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
    public class DeliveryTableSubDao : AbstractDao<DeliveryTableSub>
    {
        /// <summary>
        /// 데이터입력
        /// </summary>
        /// <returns></returns>
        public void InsertDeliveryTableSub(DeliveryTableSub entity)
        {
            base.Insert(entity, "tbl_Delivery_Sub");
        }
        /// <summary>
        /// 납품확인서 서브상품
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<DeliveryTableSub> SelectSubList(Int64 deliverykey, string companycode, LanguageType? lType)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("deliverykey", deliverykey);
            ParameterAdd("companycode", companycode);
            ParameterAdd("tblname", "productSpec");

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" a.*,b.productname ");
            if (Object.Equals(lType,LanguageType.Korea))
            {
                sb.Append(" ,c.codename_k as productspec_disp ");
            }
            else
            {
                sb.Append(" ,c.codename_j as productspec_disp ");
            }
            sb.Append(" FROM dbo.tbl_Delivery_sub a inner join tbl_productInfo b on a.productindex=b.idx,tbl_Codemaster c ");
            sb.Append(" WHERE a.state = @state ");
            sb.Append(" AND a.deliverykey = @deliverykey ");
            sb.Append(" AND a.companycode = @companycode ");
            sb.Append(" AND a.productspec = c.codekey ");
            sb.Append(" AND c.tblname=@tblname ");
            sb.Append(" order by a.idx desc");

            return Select(sb.ToString(), GetParameter());
        }
    }
}