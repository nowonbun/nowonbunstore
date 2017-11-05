using System;
using System.Collections.Generic;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Master
{
    public class LanguageMap
    {
        private static LanguageMap instance = null;
        private Dictionary<String, LanguagePack> flyweight = new Dictionary<string, LanguagePack>();
        public static LanguageMap Instance()
        {
            if (instance == null)
            {
                instance = new LanguageMap();
            }
            return instance;
        }
        private LanguageMap()
        {

        }
        public LanguagePack GetLanguage(String pPath, String Controller, String Action, LanguageType? pType)
        {
            String key = Controller + Action + pType.Value;
            if (!flyweight.ContainsKey(key))
            {
                flyweight.Add(key, new LanguagePack(pPath, Controller, Action, pType));
            }
            return flyweight[key];
        }
    }
}