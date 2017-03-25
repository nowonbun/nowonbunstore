using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;
using log4net;

namespace Household.Models.Master
{
    public class CategoryMaster : ResourceAllocationEntity
    {
        [ResourceDao]
        protected ICtgryDao ctgryDao;

        private ILog logger = LogManager.GetLogger(typeof(CategoryMaster));
        private Ctgry[] list = null;

        public CategoryMaster()
        {
            list = ctgryDao.SelectAll().ToArray();
        }

        public IList<Ctgry> GetAll()
        {
            return list.ToList();
        }

        public String GetCategoryNameByCode(String cd)
        {
            try
            {
                return (from l in list where String.Equals(l.Cd, cd) select l.Nm).First();
            }
            catch (Exception e)
            {
                logger.Error("CategoryMaster error : Nothing that you looked up.");
                logger.Error(e);
                return null;
            }
        }
    }
}