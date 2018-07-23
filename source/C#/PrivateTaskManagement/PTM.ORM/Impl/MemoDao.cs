using System;
using System.Collections.Generic;
using System.Data.OleDb;
using PTM.ORM.Dao;
using PTM.ORM.Entity;
using PTM.ORM.Common;
using System.Linq;

namespace PTM.ORM.Impl
{
    class MemoDao : Dao<Memo>, IMemoDao
    {
        public MemoDao(Database db) : base(db.GetConnetcion())
        {

        }
        public int Delete(Memo entity)
        {
            return base.DeleteByEntity(entity);
        }

        public int Insert(Memo entity)
        {
            return base.InsertByEntity(entity);
        }

        public int InsertAndScope(Memo entity)
        {
            return base.InsertByEntity(entity, true);
        }

        public IList<Memo> Select()
        {
            return base.SelectAll();
        }

        public int Update(Memo entity)
        {
            return base.UpdateByEntity(entity);
        }

        public Memo GetEneity(int idx)
        {
            return Select().Where((m) =>
            {
                return m.Idx == idx;
            }).FirstOrDefault();
        }
    }
}
