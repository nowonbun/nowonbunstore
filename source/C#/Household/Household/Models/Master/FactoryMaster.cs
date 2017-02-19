using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Models.Master
{
    public class FactoryMaster
    {
        private static FactoryMaster singleton= null;
        private IDictionary<Type, Object> flyweight = new Dictionary<Type, Object>();

        private FactoryMaster()
        {
            GetCategoryMaster();
            GetSystemCodeMaster();
            GetTypeMaster();
        }

        public static FactoryMaster Instance(){
            if (singleton == null)
            {
                singleton = new FactoryMaster();
            }
            return singleton;
        }

        public CategoryMaster GetCategoryMaster()
        {
            if (!flyweight.ContainsKey(typeof(CategoryMaster)))
            {
                flyweight.Add(typeof(CategoryMaster), new CategoryMaster());
            }
            return flyweight[typeof(CategoryMaster)] as CategoryMaster;
        }

        public SystemCodeMaster GetSystemCodeMaster()
        {
            if (!flyweight.ContainsKey(typeof(SystemCodeMaster)))
            {
                flyweight.Add(typeof(SystemCodeMaster), new SystemCodeMaster());
            }
            return flyweight[typeof(SystemCodeMaster)] as SystemCodeMaster;
        }

        public TypeMaster GetTypeMaster()
        {
            if (!flyweight.ContainsKey(typeof(TypeMaster)))
            {
                flyweight.Add(typeof(TypeMaster), new TypeMaster());
            }
            return flyweight[typeof(TypeMaster)] as TypeMaster;
        }
    }
}