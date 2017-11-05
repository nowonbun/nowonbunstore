using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Master
{
    public class NavigationMap
    {
        private static NavigationMap instance = null;
        private Dictionary<String, NavigationPack> flyweight = new Dictionary<string, NavigationPack>();
        public static NavigationMap Instance()
        {
            if (instance == null)
            {
                instance = new NavigationMap();
            }
            return instance;
        }
        private NavigationMap()
        {

        }
        public NavigationPack GetNavigation(string serverPath, string controller, string action, LanguageType pType)
        {
            String key = String.Format("{0}_{1}_{2}", controller, action, pType != null ? pType.ToString() : "");
            if (!flyweight.ContainsKey(key))
            {
                flyweight.Add(key, new NavigationPack(serverPath, controller, action, pType));
            }
            return flyweight[key];
        }
    }
}