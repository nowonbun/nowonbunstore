using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Household.Models.Entity;

namespace Household.Models.Master
{
    public class TypeMaster : List<Entity.Type>
    {
        public List<Entity.Type> GetByCategory(string cd)
        {
            return (from item in this where item.Cd.Equals(cd) select item).ToList();
        }
        public List<Entity.Type> GetBySearchCode()
        {
            return (from item in this where item.Cd.Equals("000") select item).ToList();
        }
    }
}