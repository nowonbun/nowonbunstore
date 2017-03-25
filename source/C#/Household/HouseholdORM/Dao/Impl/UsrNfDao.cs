using System;
using System.Text;
using HouseholdORM;
using System.Collections;
using System.Collections.Generic;

namespace HouseholdORM
{
    class UsrNfDao : Dao<UsrNf>, IUsrNfDao
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