using System;
using System.Collections.Generic;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class ConnectDao : AbstractDao<Connect>
    {
        public int GetConnectCount()
        {
            ParameterInit();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_Connect ");
            return SelectCount(sb.ToString(), GetParameter());
        }

        public IList<Connect> SelectConnect(int pageLimit, int page)
        {
            ParameterInit();

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_Connect ");
            sb.Append(" WHERE ");
            sb.Append(" idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_Connect order by idx desc) ");
            sb.Append(" order by idx desc");

            return Select(sb.ToString(), GetParameter());
        }
        public int InsertConnect(Connect obj)
        {
            return base.Insert(obj, "tbl_connect");
        }
    }
}