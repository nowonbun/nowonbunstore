using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class CompanyInfoDao : AbstractDao<CompanyInfo>
    {
        /// <summary>
        /// 회사정보취득하기
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public CompanyInfo SelectCompanyInfo(string companycode)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_CompanyInfo where state = '0' and companycode = @companycode");
            ParameterInit();
            ParameterAdd("companycode", companycode);
            IList<CompanyInfo> ret = base.Select(query.ToString(), GetParameter());
            if (ret.Count < 1)
            {
                return null;
            }
            return ret[0];
        }
        /// <summary>
        /// 회사정보수정
        /// </summary>
        public void Modify(CompanyInfo data, LanguageType? lType)
        {
            //삭제처리
            ParameterInit();
            ParameterAdd("companycode", data.CompanyCode);
            ParameterAdd("idx", data.Idx);
            ParameterAdd("state", Define.STATE_DELETE);
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE ");
            query.Append(" tbl_CompanyInfo ");
            query.Append(" set state = @state ");
            query.Append(" where companycode = @companycode and idx=@idx");
            base.Delete(query.ToString(), GetParameter());
            //삭제완료
            //Form데이터에 companyCode 넣기

            data.NumberJoin();
            data.State = "0";
            query.Clear();

            base.Insert(data, "tbl_CompanyInfo");
        }
    }
}