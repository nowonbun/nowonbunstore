using System;
using System.Collections.Generic;
using ScrappingORMCore;
using ScrappingServer.Entity;

namespace ScrappingServer.Dao
{
    class FactoryDao
    {
        private static FactoryDao instance = null;
        private Dictionary<Type, IDao> dic = null;
        private FactoryDao() {
            dic = new Dictionary<Type, IDao>();
        }
        private static FactoryDao GetInstance(){
            if (instance == null)
            {
                instance = new FactoryDao();
            }
            return instance;
        }
        private IDao GetDao(Type daoType)
        {
            if (!GetInstance().dic.ContainsKey(daoType))
            {
                GetInstance().dic.Add(daoType, Activator.CreateInstance(daoType) as IDao);
            }
            return GetInstance().dic[daoType];
        }
        public static ScrappingTotalDataDao GetScrappingTotalDataDao()
        {
            return GetInstance().GetDao(typeof(ScrappingTotalDataDao)) as ScrappingTotalDataDao;
        }
        public static ScrappingDataDao GetScrappingDataDao()
        {
            return GetInstance().GetDao(typeof(ScrappingDataDao)) as ScrappingDataDao;
        }
        public static ScrappingDataSubDao GetScrappingDataSubDao()
        {
            return GetInstance().GetDao(typeof(ScrappingDataSubDao)) as ScrappingDataSubDao;
        }
    }
}
