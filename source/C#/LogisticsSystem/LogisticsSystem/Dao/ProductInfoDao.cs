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
    public class ProductInfoDao : AbstractDao<ProductInfo>
    {
        public String CreateCode()
        {
            String query = " insert into codeCreater select Cast(isnull(Max(codebuffer)+1,1) as Decimal) as code,1 from codeCreater where type=1";
            Delete(query);
            query = " select Cast(Max(codebuffer)as Decimal) as code from codeCreater where type=1 ";
            DataTable dt = SelectDataTable(query);
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            Decimal code = (Decimal)dt.Rows[0][0];
            return code.ToString("0000");
        }
        /// <summary>
        /// 데이터 입력
        /// </summary>
        public void InsertProduct(ProductInfo entity)
        {
            Insert(entity, "tbl_ProductInfo");
        }
        /// <summary>
        /// 상품클릭시 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public ProductInfo SelectProduct(string code, String compcode)
        {
            ParameterInit();
            ParameterAdd("companycode", compcode);
            ParameterAdd("productcode", code);
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM tbl_ProductInfo where state = '0' and productcode = @productcode and companycode = @companycode ");
            IList<ProductInfo> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 상품 히스토리 검색(히스토리리스트에서 클릭했을때)
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public ProductInfo SelectProductHistory(int idx, string compcode)
        {
            ParameterInit();
            ParameterAdd("companycode", compcode);
            ParameterAdd("idx", idx);

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM tbl_ProductInfo where idx = @idx and companycode=@companycode  ");
            IList<ProductInfo> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 상품 삭제
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public void DeleteProduct(String productcode, string compcode)
        {
            ParameterInit();
            ParameterAdd("companycode", compcode);
            ParameterAdd("productcode", productcode);
            ParameterAdd("state", Define.STATE_DELETE);

            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE ");
            query.Append(" tbl_ProductInfo ");
            query.Append(" set state = @state ");
            query.Append(" where productcode = @productcode and companycode=@companycode ");

            base.Delete(query.ToString(), GetParameter());
        }
        /// <summary>
        /// 상품 검색
        /// Database - CompanyCode Binding OK!
        /// ltype 부분은 SE가 설정하므로 문제없다.
        /// </summary>
        public ProductInfo SelectProduct(int idx, string compcode, LanguageType? lType)
        {
            ParameterInit();
            ParameterAdd("companycode", compcode);
            ParameterAdd("idx", idx);
            ParameterAdd("state", Define.STATE_NORMAL);

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * ");
            query.Append(" FROM tbl_ProductInfo ");
            query.Append(" WHERE state = @state ");
            query.Append(" AND idx = @idx ");
            query.Append(" AND companycode=@companycode ");

            IList<ProductInfo> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 상품리스트검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <returns></returns>
        public IList<ProductInfo> GetProductNameList(String companycode)
        {
            ParameterInit();
            ParameterAdd("companycode", companycode);
            ParameterAdd("state", Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" * ");
            sb.Append(" FROM tbl_ProductInfo ");
            sb.Append(" WHERE state = @state and companycode = @companycode ");
            sb.Append(" order by idx desc");
            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 상품검색DAO
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<ProductInfo> SelectList(int pageLimit, int page, String compcode)
        {
            ParameterInit();
            ParameterAdd("companyCode", compcode);
            ParameterAdd("state", Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_ProductInfo ");
            sb.Append(" WHERE state = @state and companyCode=@companyCode");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_ProductInfo WHERE state = @state and companyCode=@companyCode order by idx desc) ");
            sb.Append(" order by idx desc");
            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 상품테이블 전체 갯수 구하는 함수(페이징용)
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int SelectProductInfoCount(String compcode)
        {
            ParameterInit();
            ParameterAdd("companyCode", compcode);
            ParameterAdd("state", Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_ProductInfo ");
            sb.Append(" WHERE state = @state and companyCode=@companyCode ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 상품 이력 리스트 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<ProductInfo> SearchProductInfoHistory(int pageLimit, int page, string code, String compcode)
        {
            ParameterInit();
            ParameterAdd("productcode", code);
            ParameterAdd("companyCode", compcode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_ProductInfo ");
            sb.Append(" WHERE productcode = @productcode and companyCode=@companyCode ");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_ProductInfo WHERE productcode = @productcode and companyCode=@companyCode order by idx desc) ");
            sb.Append(" order by idx desc");

            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 상품 이력 리스트 총 개수(페이징용)
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public int GetProductInfoHistoryCount(String code, String compcode)
        {
            ParameterInit();
            ParameterAdd("code", code);
            ParameterAdd("companyCode", compcode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_ProductInfo ");
            sb.Append(" WHERE productcode = @code and companyCode=@companyCode ");
            return SelectCount(sb.ToString(), GetParameter());
        }
    }
}