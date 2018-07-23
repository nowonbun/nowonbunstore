using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTM.ORM.Common;
using PTM.ORM.Entity;

namespace PTM.ORM.Dao
{
    public interface ITaskDao : IDao
    {
        IList<Task> Select();

        int Insert(Task entity);

        int InsertAndScope(Task entity);

        int Update(Task entity);

        int Delete(Task entity);

        Task GetEntity(int idx);
    }
}
