using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using MVC_Hibernate.Entity;
using System.IO;
namespace MVC_Hibernate.Dao
{
    public class FactoryDao
    {
        private Configuration _configuration;
        private ISessionFactory _sessionFactory;
        private ISession _session;
        private Dictionary<Type, IDao> flyweight = new Dictionary<Type, Dao.IDao>();
        private static FactoryDao _factory;

        private FactoryDao()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            var hmbFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dao\\Config"), "*.hbm.xml", SearchOption.TopDirectoryOnly);
            foreach (var file in hmbFiles)
            {
                _configuration.AddFile(file);
            }
        }

        public void Open()
        {
            Close();
            _sessionFactory = _configuration.BuildSessionFactory();
            _session = _sessionFactory.OpenSession();
        }

        public void Close()
        {
            if (_session != null && _session.IsOpen)
            {
                _session.Close();
            }
            if (_sessionFactory != null && !_sessionFactory.IsClosed)
            {
                _sessionFactory.Close();
            }
        }

        public static UsertableDao GetUsertableDao()
        {
            if(_factory == null)
            {
                _factory = new FactoryDao();
                _factory.Open();
            }
            if (!_factory.flyweight.ContainsKey(typeof(UsertableDao)))
            {
                _factory.flyweight.Add(typeof(UsertableDao), new UsertableDao(_factory._session));
            }
            return _factory.flyweight[typeof(UsertableDao)] as UsertableDao;
        }
    }
}