using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Newtonsoft.Json;
using log4net;

namespace Household.Models.Master
{
    public class FactoryMaster
    {
        private ILog logger;
        private static FactoryMaster instance = null;
        private TypeMaster typemaster;
        private CategoryMaster categorymaster;
        private SystemDataMaster systemdatamaster;

        private FactoryMaster()
        {
            try
            {
                logger = LogManager.GetLogger(this.GetType());
                String json = HttpConnector.GetInstance().GetDataRequest("GetMaster.php", new Dictionary<String, Object>() { { "a", 0 } });
                Dictionary<String, Object> map = JsonConvert.DeserializeObject<Dictionary<String, Object>>(json);

                typemaster = JsonConvert.DeserializeObject<TypeMaster>(map["TP"].ToString());
                categorymaster = JsonConvert.DeserializeObject<CategoryMaster>(map["CATEGORY"].ToString());
                systemdatamaster = JsonConvert.DeserializeObject<SystemDataMaster>(map["SYSTEMDATA"].ToString());
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.Error("The master is not loaded.");
            }
        }
        public static void CreateInstance()
        {
            if (instance == null)
            {
                instance = new FactoryMaster();
            }
        }
        public static FactoryMaster Instance()
        {
            CreateInstance();
            return instance;
        }
        public TypeMaster GetTypeMaster()
        {
            return this.typemaster;
        }
        public CategoryMaster GetCategoryMaster()
        {
            return this.categorymaster;
        }
        public SystemDataMaster GetSystemDataMaster()
        {
            return this.systemdatamaster;
        }
    }
}
