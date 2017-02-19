using System;
using System.Text;
using Household.Models.Entity;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;
using Household.Common;

namespace Household.Dao
{
    public class UsrNfDao : AbstractDao<UsrNf>
    {
        public UsrNf SelectForSign(String groupId, String id, String pw)
        {

            StringBuilder query = new StringBuilder();
            query.Append(" select * from usr_nf where GRPD=@groupId and USRD=@id and PSWRD = @pw; ");
            IList<UsrNf> ret = base.Select(query.ToString(),
                                            CreateParameter("@groupId", groupId),
                                            CreateParameter("@id", id),
                                            CreateParameter("@pw", pw));
            if (ret.Count > 0)
            {
                return ret[0];
            }
            return null;
        }
    }
}