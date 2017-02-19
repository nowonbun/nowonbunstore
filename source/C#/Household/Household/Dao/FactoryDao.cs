using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;
using log4net;

namespace Household.Dao
{
    public class FactoryDao
    {
        private ILog logger = LogManager.GetLogger(typeof(FactoryDao));
        private IDictionary<Type, IDao> flyweight = new Dictionary<Type, IDao>();
        private static FactoryDao singleton = null;

        /// <summary>
        /// This class is not allocated to external.i.e. It means pattern of singleton
        /// </summary>
        private FactoryDao()
        {
            logger.Info("Started up Factory of dao.");
            GetCnctLgDao();
            GetCtgryDao();
            GetCtgryDao();
            GetHshldDao();
            GetHshldLogDao();
            GetSysDtDao();
            GetTpDao();
            GetUsrNfDao();
        }
        /// <summary>
        /// Singleton pattern
        /// </summary>
        /// <returns>FactoryDao</returns>
        public static FactoryDao Instance()
        {
            if (singleton == null)
            {
                singleton = new FactoryDao();
            }
            return singleton;
        }
        public CnctLgDao GetCnctLgDao()
        {
            if(!flyweight.ContainsKey(typeof(CnctLgDao))){
                flyweight.Add(typeof(CnctLgDao),new CnctLgDao());
                logger.Info("Registered the Dao of the connect Log");
            }
            logger.Info("Acquired the Dao of the connect Log");
            return flyweight[typeof(CnctLgDao)] as CnctLgDao;
        }
        public CtgryDao GetCtgryDao()
        {
            if (!flyweight.ContainsKey(typeof(CtgryDao)))
            {
                flyweight.Add(typeof(CtgryDao), new CtgryDao());
                logger.Info("Registered the Dao of the category");
            }
            logger.Info("Acquired the Dao of the category");
            return flyweight[typeof(CtgryDao)] as CtgryDao;
        }
        
        public GrpNfDao GetGrpNfDao()
        {
            if (!flyweight.ContainsKey(typeof(GrpNfDao)))
            {
                flyweight.Add(typeof(GrpNfDao), new GrpNfDao());
            }
            return flyweight[typeof(GrpNfDao)] as GrpNfDao;
        }
        public HshldDao GetHshldDao()
        {
            if (!flyweight.ContainsKey(typeof(HshldDao)))
            {
                flyweight.Add(typeof(HshldDao), new HshldDao());
            }
            return flyweight[typeof(HshldDao)] as HshldDao;
        }
        public HshldLogDao GetHshldLogDao()
        {
            if (!flyweight.ContainsKey(typeof(HshldLogDao)))
            {
                flyweight.Add(typeof(HshldLogDao), new HshldLogDao());
            }
            return flyweight[typeof(HshldLogDao)] as HshldLogDao;
        }
        public SysDtDao GetSysDtDao()
        {
            if (!flyweight.ContainsKey(typeof(SysDtDao)))
            {
                flyweight.Add(typeof(SysDtDao), new SysDtDao());
            }
            return flyweight[typeof(SysDtDao)] as SysDtDao;
        }
        public TpDao GetTpDao()
        {
            if (!flyweight.ContainsKey(typeof(TpDao)))
            {
                flyweight.Add(typeof(TpDao), new TpDao());
            }
            return flyweight[typeof(TpDao)] as TpDao;
        }
        public UsrNfDao GetUsrNfDao()
        {
            if (!flyweight.ContainsKey(typeof(UsrNfDao)))
            {
                flyweight.Add(typeof(UsrNfDao), new UsrNfDao());
            }
            return flyweight[typeof(UsrNfDao)] as UsrNfDao;
        }
    }
}