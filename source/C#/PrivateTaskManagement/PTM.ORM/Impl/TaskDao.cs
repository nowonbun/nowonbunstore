using System;
using System.Collections.Generic;
using System.Data.OleDb;
using PTM.ORM.Dao;
using PTM.ORM.Entity;
using PTM.ORM.Common;
using System.Linq;

namespace PTM.ORM.Impl
{
    class TaskDao : Dao<Task>, ITaskDao
    {
        public TaskDao(Database db) : base(db.GetConnetcion())
        {

        }
        public int Delete(Task entity)
        {
            return base.DeleteByEntity(entity);
        }

        public int Insert(Task entity)
        {
            return base.InsertByEntity(entity);
        }

        public int InsertAndScope(Task entity)
        {
            return base.InsertByEntity(entity, true);
        }

        public IList<Task> Select()
        {
            return base.SelectAll();
        }

        public int Update(Task entity)
        {
            return base.UpdateByEntity(entity);
        }

        public Task GetEntity(int idx)
        {
            return Select().Where(t => { return t.Idx == idx; }).FirstOrDefault();
        }
    }
}
