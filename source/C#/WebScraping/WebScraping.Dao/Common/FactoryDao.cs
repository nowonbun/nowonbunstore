using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Dao.Impl;
using WebScraping.Dao.Attribute;
using WebScraping.Dao.Interface;

namespace WebScraping.Dao.Common
{
    public sealed class FactoryDao
    {
        private static FactoryDao instance = null;

        public static FactoryDao CreateInstance(String connectionString, String tempPath)
        {
            if (instance != null)
            {
                throw new Exception("already allocaion!");
            }
            instance = new FactoryDao(connectionString, tempPath);
            return instance;
        }
        public static FactoryDao GetInstance()
        {
            if (instance == null)
            {
                throw new Exception("not allocaion!");
            }
            return instance;
        }

        private IDictionary<Type, IDao> flyweight = new Dictionary<Type, IDao>();

        private String connectionString;
        private FactoryDao(String connectionString, String temppath)
        {
            this.connectionString = connectionString;

            flyweight.Add(typeof(IScrapingCommonDataDao), new ScrapingCommonDataDao());
            flyweight.Add(typeof(IScrapingPackageDataDao), new ScrapingPackageDataDao());
            flyweight.Add(typeof(IScrapingStatusDao), new ScrapingStatusDao());
            flyweight.Add(typeof(IScrapingStatusTypeDao), new ScrapingStatusTypeDao());

            Parallel.ForEach(flyweight, item =>
            {
                item.Value.SetConnectionString(connectionString);
                item.Value.SetCsvPath(temppath);
            });
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
                field.SetValue(obj, FactoryDao.GetInstance().GetDao(obj.GetType()));
                //ImplementDao impl = field.ReflectedType.GetCustomAttribute(typeof(ImplementDao)) as ImplementDao;
                //field.SetValue(obj, FactoryDao.GetInstance().GetDao());
            });
        }
        public T GetDao<T>()
        {
            return (T)flyweight[typeof(T)];
        }
        public Object GetDao(Type type)
        {
            return flyweight[type];
        }
        /*public Object GetDao(String className)
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
        }*/
    }
}
