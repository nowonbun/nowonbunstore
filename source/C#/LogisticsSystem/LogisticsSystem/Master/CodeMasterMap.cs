using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using LogisticsSystem.Dao;

namespace LogisticsSystem.Master
{
    public class CodeMasterMap
    {
        private static CodeMasterMap instance = null;
        private Dictionary<String, IList<CodeMaster>> master = new Dictionary<string, IList<CodeMaster>>();
        private CodeMasterDao dao = FactoryDao.Instance().GetCodeMasterDao();
        public static CodeMasterMap Instance()
        {
            if(instance == null)
            {
                instance = new CodeMasterMap();
            }
            return instance;
        }
        private CodeMasterMap()
        {
            
        }
        public IList<CodeMaster> GetCodeMaster(String keyName,LanguageType? ltype)
        {
            if (!master.ContainsKey(keyName))
            {
                master.Add(keyName, dao.SelectCodeMaster(keyName));
            }
            return ConvertLanguage(master[keyName], ltype);
        }
        /// <summary>
        /// 언어 변환
        /// </summary>
        /// <param name="ltype"></param>
        private IList<CodeMaster> ConvertLanguage(IList<CodeMaster> list, LanguageType? ltype)
        {
            foreach (CodeMaster l in list)
            {
                l.Trans(ltype);
            }
            return list;
        }
    }
}