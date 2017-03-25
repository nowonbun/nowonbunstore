using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HouseholdORM
{
    public sealed class FactoryDao
    {
        private static FactoryDao instance = null;

        public static FactoryDao CreateInstance(String connectionString)
        {
            if (instance != null)
            {
                throw new HouseholdORMException("already allocaion!");
            }
            instance = new FactoryDao(connectionString);
            return instance;
        }
        public static FactoryDao GetInstance()
        {
            if (instance == null)
            {
                throw new HouseholdORMException("not allocaion!");
            }
            return instance;
        }

        private IDictionary<String, IDao> flyweight = new Dictionary<String, IDao>();

        private String connectionString;
        private FactoryDao(String connectionString)
        {
            this.connectionString = connectionString;
        }
        public void AllocResource(object obj)
        {
            var fields = from field in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         let attrs = field.GetCustomAttributes()
                         from attr in attrs
                         where attr is ResourceDao
                         select field;

            Parallel.ForEach(fields, field =>
            {
                ImplementDao impl = field.ReflectedType.GetCustomAttribute(typeof(ImplementDao)) as ImplementDao;
                field.SetValue(obj, FactoryDao.GetInstance().GetDao(impl.ClassName));
            });
        }
        public Object GetDao(String className)
        {
            lock (flyweight)
            {
                if (!flyweight.ContainsKey(className))
                {
                    IDao cls = Activator.CreateInstance(Type.GetType(className)) as IDao;
                    cls.SetConnectionString(connectionString);
                    flyweight.Add(className, cls);
                }
            }
            return flyweight[className];
        }
    }
}
