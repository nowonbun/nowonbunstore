using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Dao;
using Household.Models.Entity;
using log4net;

namespace Household.Models.Master
{
    public class TypeMaster
    {
        private ILog logger = LogManager.GetLogger(typeof(TypeMaster));
        private Tp[] list = null;
        public TypeMaster()
        {
            TpDao dao = FactoryDao.Instance().GetTpDao();
            list = dao.SelectAll().ToArray();
        }

        public IList<Tp> GetAll()
        {
            return list.ToList();
        }
        public IList<Tp> GetByCategoryCode(String cd)
        {
            try
            {
                return (from l in list where String.Equals(l.Cd, cd) select l).ToList();
            }
            catch (Exception e)
            {
                logger.Error("TypeMaster error : Nothing that you looked up.");
                logger.Error(e);
                return null;
            }
        }

        public String GetTypeNameByCode(String tp)
        {
            try
            {
                return (from l in list where String.Equals(l.TP, tp) select l.Nm).First();
            }
            catch (Exception e)
            {
                logger.Error("TypeMaster error : Nothing that you looked up.");
                logger.Error(e);
                return null;
            }
        }

        public IList<Tp> GetSearchCode()
        {
            try
            {
                return (from l in list where String.Equals(l.Cd, "000") select l).ToList();
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