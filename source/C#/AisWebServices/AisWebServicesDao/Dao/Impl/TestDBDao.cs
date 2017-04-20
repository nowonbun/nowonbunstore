using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisWebServicesDao
{
    class TestDBDao : Dao<TestDB>, ITestDBDao
    {
        public IList<TestDB> select()
        {
            return base.SelectByEntity(null);
        }
    }
}
