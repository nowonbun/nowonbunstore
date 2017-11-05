using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class CodeMasterDao : AbstractDao<CodeMaster>
    {
        public IList<CodeMaster> SelectCodeMaster(String keyName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" * ");
            sb.Append(" FROM TBL_CODEMASTER ");
            sb.Append(" WHERE TBLNAME = @tblname ");

            base.ParameterInit();
            base.ParameterAdd("@tblname", keyName);
            return Select(sb.ToString(), base.GetParameter());
        }
        
    }
}