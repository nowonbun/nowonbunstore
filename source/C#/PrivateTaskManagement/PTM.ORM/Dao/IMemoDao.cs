using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTM.ORM.Common;
using PTM.ORM.Entity;

namespace PTM.ORM.Dao
{
    public interface IMemoDao : IDao
    {
        IList<Memo> Select();

        int Insert(Memo entity);

        int InsertAndScope(Memo entity);

        int Update(Memo entity);

        int Delete(Memo entity);

        Memo GetEneity(int idx);
    }
}
