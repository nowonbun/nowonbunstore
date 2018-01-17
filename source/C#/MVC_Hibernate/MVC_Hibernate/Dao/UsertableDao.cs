using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using MVC_Hibernate.Entity;

namespace MVC_Hibernate.Dao
{
    public class UsertableDao : AbstractDao<Usertable>, IDao
    {
        public UsertableDao(ISession session): base(session)
        {
           
        }
        public Usertable SelectId(String id, String pw)
        {
            return Transaction((session) =>
            {
                return session.Query<Usertable>().Where(p => String.Equals(p.ID, id) && String.Equals(p.PW, pw)).FirstOrDefault();
            });
        }
    }
}