using System;
using HouseholdORM;
using System.Text;

namespace HouseholdORM
{
    class CnctLgDao : Dao<CnctLg>, ICnctLgDao
    {
        public void InsertToSignin(String groupId, String id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("INSERT iNTO CNCT_LG (GRPD,USRD,CNCTM) VALUES(@GRPD,@USRD,NOW())");
            base.ExecuteNotResult(query.ToString(), CreateParameter("@GRPD", groupId), CreateParameter("@USRD", id));
        }
    }
}