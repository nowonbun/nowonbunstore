using System;
using System.Collections.Generic;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class CargoDao : AbstractDao<Cargo>
    {
        /// 재고검색
        /// Database - CompanyCode Binding OK!
        public IList<Cargo> SelectCargoByCompanyCode(String companycode)
        {
            ParameterInit();
            ParameterAdd("companycode", companycode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" select a.*,b.productname from ( ");
            sb.Append(" SELECT ");
            sb.Append(" productindex, ");
            sb.Append(" sum(productInput)-sum(productoutput) as ProductAmount, ");
            sb.Append(" convert(decimal(18,0),0) as ProductAvgPrice ");
            sb.Append(" from tbl_cargo ");
            sb.Append(" where companycode=@companycode ");
            sb.Append(" group by productindex ");
            sb.Append(" ) a inner join tbl_ProductInfo b ");
            sb.Append(" on a.productindex = b.idx ");
            sb.Append(" order by b.idx desc ");

            return Select(sb.ToString(), GetParameter());
        }
        /// 수불입검색수
        /// Database - CompanyCode Binding OK!
        public int GetCargoListCount(String companycode)
        {
            ParameterInit();
            ParameterAdd("companycode", companycode);
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_cargo where companycode=@companycode and state=@state ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// 수불입검색
        /// Database - CompanyCode Binding OK!
        public IList<Cargo> SelectCargoList(int pageLimit, int page, String companycode)
        {
            ParameterInit();
            ParameterAdd("companycode", companycode);
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " a.*,b.productname ");
            sb.Append(" FROM tbl_cargo a inner join tbl_ProductInfo b on a.productIndex = b.idx");
            sb.Append(" WHERE a.idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_cargo where a.companycode=@companycode and a.state=@state) ");
            sb.Append(" and a.companycode=@companycode and a.state=@state ");
            sb.Append(" order by a.idx desc");

            return Select(sb.ToString(), GetParameter());
        }
        public void InsertCargo(Cargo entity)
        {
            base.Insert(entity, "tbl_cargo");
        }
    }
}