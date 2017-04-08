using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisWebServicesDao
{
    [ImplementDao("AisWebServicesDao.TestDBDao")]
    public interface ITestDBDao
    {
        IList<TestDB> select();
    }
}
