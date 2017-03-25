using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;
using log4net;

namespace Household.Models.Master
{
    public class SystemCodeMaster : ResourceAllocationEntity
    {
        [ResourceDao]
        protected ISysDtDao sysDtDao;

        private ILog logger = LogManager.GetLogger(typeof(SystemCodeMaster));
        private SysDt[] list = null;

        public SystemCodeMaster()
        {
            list = sysDtDao.SelectAll().ToArray();
        }

        public IList<SysDt> GetAll()
        {
            return list.ToList();
        }

        public String GetByKeyCode(String kycd)
        {
            try
            {
                return (from l in list where String.Equals(l.Kycd, kycd) select l.Dt).First();
            }
            catch (Exception e)
            {
                logger.Error("SystemCodeMaster error : Nothing that you looked up.");
                logger.Error(e);
                return null;
            }

        }
    }
}