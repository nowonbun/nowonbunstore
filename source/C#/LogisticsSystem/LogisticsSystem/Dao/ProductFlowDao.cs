using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class ProductFlowDao : AbstractDao<ProductFlow>
    {
        /// <summary>
        /// 데이터 입력
        /// </summary>
        public int InsertProductFlow(ProductFlow entity)
        {
            return base.Insert(entity, "tbl_ProductFlow");
        }
        /// <summary>
        /// 입고등록
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public void ApproveProduct(Int64 idx, String companycode, int state)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", companycode);
            ParameterAdd("state", state);

            StringBuilder query = new StringBuilder();
            query.Append(" Update tbl_ProductFlow ");
            query.Append(" set state = @state ");
            query.Append(" where idx=@idx and companycode=@companycode");

            base.Update(query.ToString(), GetParameter());
        }
        public int ApproveProductRelease(Int64 idx)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" Update tbl_ProductFlow ");
            query.Append(" set state = '4' ");
            query.Append(" where idx = @idx ");
            ParameterInit();
            ParameterAdd("idx", idx);
            return base.Update(query.ToString(), GetParameter());
        }
        /// <summary>
        /// 데이터 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public ProductFlow SelectProductFlow(Int64 idx, String companycode)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", companycode);

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT A.*,B.productname FROM tbl_ProductFlow A inner join tbl_productInfo B on A.productIndex = B.idx ");
            query.Append(" where A.idx = @idx and A.companycode=@companycode ");

            IList<ProductFlow> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 재고 전체 검색 수
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int GetProductFlowCount(int state, String companycode)
        {
            StringBuilder sb = new StringBuilder();
            ParameterInit();
            ParameterAdd("state", state);
            ParameterAdd("companycode", companycode);
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_ProductFlow a inner join tbl_ProductInfo b on a.productIndex = b.idx ");
            sb.Append(" WHERE a.companycode = @companycode ");
            sb.Append(" AND a.state = @state ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 재고 전체 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<ProductFlow> SelectFlow(int pageLimit, int page, int state, String companycode)
        {
            StringBuilder sb = new StringBuilder();
            ParameterInit();
            ParameterAdd("state", state);
            ParameterAdd("companycode", companycode);

            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " a.*,b.productname ");
            sb.Append(" FROM tbl_ProductFlow a inner join tbl_ProductInfo b on a.productIndex = b.idx ");
            sb.Append(" WHERE a.state = @state and a.companycode = @companycode ");
            sb.Append(" AND a.idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_ProductFlow WHERE state = @state and companycode = @companycode order by idx desc) ");
            sb.Append(" order by a.idx desc");

            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 전체 검색 수
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int CountFlowList(String companycode, int state1, int state2, int state3)
        {
            ParameterInit();
            ParameterAdd("companycode", companycode);
            ParameterAdd("state1", state1);
            ParameterAdd("state2", state2);
            ParameterAdd("state3", state3);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_ProductFlow ");
            sb.Append(" WHERE state in (@state1,@state2,@state3) and companycode = @companycode ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 전체 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<ProductFlow> SelectFlowList(int pageLimit, int page, String companycode, int state1, int state2, int state3)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " a.*,b.productname ");
            sb.Append(" FROM tbl_ProductFlow a inner join tbl_ProductInfo b on a.productIndex = b.idx ");
            sb.Append(" WHERE a.state in (@state1,@state2,@state3) and a.companycode = @companycode ");
            sb.Append(" AND a.idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_ProductFlow ");
            sb.Append(" WHERE a.state in (@state1,@state2,@state3) and a.companycode = @companycode) ");
            sb.Append(" order by a.idx desc");

            return Select(sb.ToString(), GetParameter());
        }
    }
}