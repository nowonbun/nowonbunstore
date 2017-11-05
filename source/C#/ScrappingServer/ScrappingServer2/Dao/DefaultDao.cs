using System;
using ScrappingORMCore;
using ScrappingServer.Entity;
using System.Reflection;

namespace ScrappingServer.Dao
{
    class DefaultDao<T> : Dao<T>
    {
        public override String GetConnectionString()
        {
            return "data source = 27.1.43.70; initial catalog=GSM; user id=sa; password=dkagh12!@";
        }
        public int Insert(T entity, bool scope = false)
        {
            return base.insert(entity, scope);
        }
        
    }
}
