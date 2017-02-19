using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Dao;
using Household.Models.Entity;
using log4net;

namespace Household.Models.Master
{
    public class CategoryMaster 
    {
        private ILog logger = LogManager.GetLogger(typeof(CategoryMaster));
        private Ctgry[] list = null;

        public CategoryMaster()
        {
            CtgryDao dao = FactoryDao.Instance().GetCtgryDao();
            list = dao.SelectAll().ToArray();
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