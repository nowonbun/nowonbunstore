using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogisticsSystem.App_Code
{
    public abstract class AbstractModel : ICloneable
    {
        private IList<FieldInfo> keys = new List<FieldInfo>();
        public AbstractModel()
        {
            FieldInfo[] field = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo f in field)
            {
                if (Object.Equals(f, "keys"))
                {
                    continue;
                }
                if (f.Name.IndexOf("<") != -1 && f.Name.IndexOf(">") != -1)
                {
                    continue;
                }
                if (KeySetting(f.Name))
                {
                    continue;
                }
                keys.Add(f);
            }
        }
        protected abstract bool KeySetting(String columnName);
        public IList<FieldInfo> GetKeys()
        {
            return keys;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}