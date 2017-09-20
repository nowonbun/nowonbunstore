using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Common
{
    public abstract class Allocation : Entity
    {
        public Allocation()
        {
            var fields = from field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         let attrs = field.GetCustomAttributes()
                         from attr in attrs
                         where attr is ResourceDao
                         select field;

            foreach (var field in fields)
            {
                ImplementDao impl = field.FieldType.GetCustomAttribute(typeof(ImplementDao)) as ImplementDao;
                field.SetValue(this, FactoryDao.GetInstance().GetDao(impl.ClassName));
            }
        }
    }
}
