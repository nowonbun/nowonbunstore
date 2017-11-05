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
    public class CustomerInfoDao : AbstractDao<CustomerInfo>
    {
        /// <summary>
        /// 코드생성기
        /// </summary>
        /// <returns></returns>
        public String CreateCode()
        {
            String query = " insert into codeCreater select Cast(isnull(Max(codebuffer)+1,1) as Decimal) as code,2 from codeCreater where type=2";
            Delete(query);
            query = " select Cast(Max(codebuffer)as Decimal) as code from codeCreater where type=2 ";
            DataTable dt = SelectDataTable(query);
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            Decimal code = (Decimal)dt.Rows[0][0];
            return code.ToString("0000");
        }

        /// <summary>
        /// 고객 리스트 총 개수
        /// </summary>
        public int GetCustomerInfoCount(string compcode)
        {
            ParameterInit();
            ParameterAdd("companycode", compcode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_Customer ");
            sb.Append(" WHERE state = '0' and companycode=@companycode");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 고객 리스트검색 쿼리
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<CustomerInfo> SelectList(int pageLimit, int page, String compcode)
        {
            ParameterInit();
            ParameterAdd("companycode", compcode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_Customer ");
            sb.Append(" WHERE state = '0' and companycode=@companycode");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_Customer WHERE state = '0' and companycode=@companycode order by idx desc) ");
            sb.Append(" order by idx desc");
            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 히스토리 리스트에서 검색시
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public IList<CustomerInfo> SearchCustomerInfoHistory(int pageLimit, int page, string code, String compcode)
        {
            ParameterInit();
            ParameterAdd("customercode", code);
            ParameterAdd("companycode", compcode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_Customer ");
            sb.Append(" WHERE customercode = @customercode and companycode=@companycode ");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_Customer WHERE customercode = @customercode and companycode=@companycode order by idx desc) ");
            sb.Append(" order by idx desc");

            return Select(sb.ToString(), GetParameter());
        }
        public int GetCustomerInfoHistoryCount(String code, String companycode)
        {
            ParameterInit();
            ParameterAdd("code", code);
            ParameterAdd("companycode", companycode);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_Customer ");
            sb.Append(" WHERE customercode = @code and companycode = @companycode");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 발주사 리스트
        /// Database - CompanyCode Binding OK!
        /// </summary>
        /// <returns></returns>
        public IList<CustomerInfo> SelectByOrderCompList(String companycode)
        {
            ParameterInit();
            ParameterAdd("customerType", "001");
            ParameterAdd("companycode", companycode);
            ParameterAdd("state", Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" * ");
            sb.Append(" FROM tbl_Customer ");
            sb.Append(" WHERE customerType = @customerType and state = @state and companycode=@companycode ");
            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 수주사리스트
        /// </summary>
        /// <returns></returns>
        public IList<CustomerInfo> SelectByInorderCompList(String companycode)
        {
            ParameterInit();
            ParameterAdd("customerType", "002");
            ParameterAdd("companycode", companycode);
            ParameterAdd("state", Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" * ");
            sb.Append(" FROM tbl_Customer ");
            sb.Append(" WHERE customerType = @customerType and state = @state and companycode=@companycode ");
            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 고객 입력 쿼리
        /// </summary>
        public int InsertCustmer(CustomerInfo entity)
        {
            return Insert(entity, "tbl_Customer");
        }

        /// <summary>
        /// 고객검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public CustomerInfo SelectCustomer(string code, String compcode)
        {
            ParameterInit();
            ParameterAdd("customercode", code);
            ParameterAdd("companycode", compcode);
            ParameterAdd("state", Define.STATE_NORMAL);
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_Customer where state = @state and customercode = @customercode and companycode=@companycode");
            IList<CustomerInfo> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            CustomerInfo info = list[0];
            try
            {
                String[] buffer = info.CustomerPostNumber.Split('-');
                info.CustomerPostNumber1 = buffer[0];
                info.CustomerPostNumber2 = buffer[1];
                buffer = info.CustomerTaxViewerPostNumber.Split('-');
                info.CustomerTaxViewerPostNumber1 = buffer[0];
                info.CustomerTaxViewerPostNumber2 = buffer[1];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("우편번호 분할시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }
            return info;
        }
        /// <summary>
        /// 고객검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public CustomerInfo SelectCustomer(int idx, String compcode)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", compcode);
            ParameterAdd("state", Define.STATE_NORMAL);
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_Customer where state = @state and idx = @idx and companycode=@companycode ");
            IList<CustomerInfo> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            CustomerInfo info = list[0];
            try
            {
                String[] buffer = info.CustomerPostNumber.Split('-');
                info.CustomerPostNumber1 = buffer[0];
                info.CustomerPostNumber2 = buffer[1];
                buffer = info.CustomerTaxViewerPostNumber.Split('-');
                info.CustomerTaxViewerPostNumber1 = buffer[0];
                info.CustomerTaxViewerPostNumber2 = buffer[1];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("고객 검색시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }

            return info;
        }
        /// <summary>
        /// 고객검색
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public CustomerInfo SelectCustomerHistory(int idx, String compcode)
        {
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("companycode", compcode);
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM tbl_Customer where idx = @idx and companycode=@companycode ");
            IList<CustomerInfo> list = base.Select(query.ToString(), GetParameter());
            if (list.Count < 1)
            {
                return null;
            }
            CustomerInfo info = list[0];
            try
            {
                String[] buffer = info.CustomerPostNumber.Split('-');
                info.CustomerPostNumber1 = buffer[0];
                info.CustomerPostNumber2 = buffer[1];
                buffer = info.CustomerTaxViewerPostNumber.Split('-');
                info.CustomerTaxViewerPostNumber1 = buffer[0];
                info.CustomerTaxViewerPostNumber2 = buffer[1];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("히스토리 검색시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }

            return info;
        }
        /// <summary>
        /// 삭제 쿼리
        /// </summary>
        public void DeleteCustomer(String code,String compcode)
        {
            ParameterInit();
            ParameterAdd("customercode", code);
            ParameterAdd("companycode", compcode);

            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE ");
            query.Append(" tbl_Customer ");
            query.Append(" set state = 1 ");
            query.Append(" where customercode = @customercode and companycode = @companycode");
            base.Delete(query.ToString(), GetParameter());
        }
    }
}