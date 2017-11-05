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
    public class DocumentDao : AbstractDao<Document>
    {
        /// <summary>
        /// 생성코드
        /// </summary>
        /// <returns></returns>
        public String CreateCode()
        {
            String query = " insert into codeCreater select Cast(isnull(Max(codebuffer)+1,1) as Decimal) as code,3 from codeCreater where type=3";
            Delete(query);
            query = " select Cast(Max(codebuffer)as Decimal) as code from codeCreater where type=3 ";
            DataTable dt = base.SelectDataTable(query);
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            Decimal code = (Decimal)dt.Rows[0][0];
            return "MU-" + code.ToString("0000000000");
        }
        public int InsertDocument(Document entity)
        {
            return Insert(entity, "tbl_Document");
        }
        /// <summary>
        /// 문서번호 검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <returns></returns>
        public Document SelectDocument(Int64 documentIndex, String documentType, String compcode)
        {
            ParameterInit();
            ParameterAdd("documentType", documentType);
            ParameterAdd("documentIndex", documentIndex);
            ParameterAdd("companycode", compcode);

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_document where documentType = @documentType and documentIndex = @documentIndex and companycode = @companycode");
            IList<Document> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }
    }
}