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
    public class OrderTableSubDao : AbstractDao<OrderTableSub>
    {
        /// <summary>
        /// 리스트저장
        /// </summary>
        public void InsertOrderSubList(IList<OrderTableSub> entities,Int64 orderkey, String creater, String companycode)
        {
            int number = 1;
            foreach (OrderTableSub entity in entities)
            {
                entity.OrderKey = orderkey;
                entity.Number = number;
                entity.CreateDate = DateTime.Now;
                entity.Creater = creater;
                entity.State = "0";
                String disp = entity.ProductSpecDisp;
                String productname = entity.ProductName;

                //TODO:수정 필요
                //DB에 없는 객체 삭제
                entity.ProductSpecDisp = null;
                entity.ProductName = null;
                //회사코드!
                entity.CompanyCode = companycode;
                InsertOrderTableSub(entity);
                //Disp객체 재 입력(Spec값임)
                entity.ProductSpecDisp = disp;
                entity.ProductName = productname;
                number++;
            }
        }
        /// <summary>
        /// 발주서 상품내용 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<OrderTableSub> SelectOrderTableSub(Int64 orderkey, String companycode, LanguageType? ltype)
        {
            ParameterInit();
            ParameterAdd("orderkey", orderkey);
            ParameterAdd("companycode", companycode);
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" a.*,b.productname ");
            sb.Append(" FROM dbo.tbl_Orderlist_sub A INNER JOIN tbl_ProductInfo B ON A.PRODUCTINDEX = B.idx ");
            sb.Append(" WHERE a.orderkey = @orderkey and A.companycode=@companycode and b.companycode=@companycode ");
            sb.Append(" order by a.idx desc");

            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 입력
        /// </summary>
        public int InsertOrderTableSub(OrderTableSub entity)
        {
            return Insert(entity, "tbl_Orderlist_sub");
        }
        /// <summary>
        /// 상태변경
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int ModifyState(Int64 idx, int state, String companycode)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("state", state);
            ParameterAdd("companycode", companycode);

            StringBuilder query = new StringBuilder();
            query.Append(" Update tbl_Orderlist_sub ");
            query.Append(" set state = @state ");
            query.Append(" where idx = @idx ");
            query.Append(" and companycode = @companycode ");

            return base.Update(query.ToString(), GetParameter());
        }
        /// <summary>
        /// 납품확인서 서브상품
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<OrderTableSub> SelectSubList(Int64 deliverykey, string companycode, LanguageType? lType)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("deliverykey", deliverykey);
            ParameterAdd("companycode", companycode);
            ParameterAdd("tblname", "productSpec");

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" a.*,b.productname ");
            if (lType == LanguageType.Korea)
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