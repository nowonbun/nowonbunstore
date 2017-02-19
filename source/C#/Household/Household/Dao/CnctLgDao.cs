using System;
using Household.Models.Entity;
using HouseholdORM;
using System.Text;

namespace Household.Dao
{
    public class CnctLgDao : AbstractDao<CnctLg>
    {
        public void InsertToSignin(String groupId, String id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("INSERT iNTO CNCT_LG (GRPD,USRD,CNCTM) VALUES(@GRPD,@USRD,NOW())");
            base.ExecuteNotResult(query.ToString(), CreateParameter("@GRPD", groupId), CreateParameter("@USRD", id));
        }
    }
}