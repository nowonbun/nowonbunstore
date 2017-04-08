using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIsSocketServices.Impl;
using System.Reflection;

namespace AIsSocketServices
{
    public class AisSocketServices
    {
        private static AisSocketServices singleton = null;
        private IAisList list = null;
        private IAisSocketServer server = null;

        private AisSocketServices() { }
        public static AisSocketServices GetInstance(){
            if (singleton == null)
            {
                singleton = new AisSocketServices();
            }
            return singleton;
        }
        public IAisSocketServer Run(){
            if (list == null)
            {
                list = new AisListImpl();
            }
            if (this.server == null)
            {
                AisSocketServerImpl server = new AisSocketServerImpl(15000);
                server.Run();
                server.SetList(list);
                this.server = server;
            }
            return this.server;
        }
        public String SetDataList<T>(IList<T> data)
        {
            var fields = from field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         let attrs = field.GetCustomAttributes()
                         from attr in attrs
                         where attr is Messgae
                         select field;
            IList<IDictionary<String, Object>> listmap = new List<IDictionary<String, Object>>();
            foreach (T entity in data)
            {
                IDictionary<String, Object> map = new Dictionary<String, Object>();
                listmap.Add(map);
                foreach (var field in fields)
                {
                    Messgae message = field.GetCustomAttribute(typeof(Messgae)) as Messgae;
                    map.Add(message.KeyName, field.GetValue(entity));
                }
            }
            return list.Push(listmap);
        }
        public String SetData<T>(T data)
        {
            var fields = from field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         let attrs = field.GetCustomAttributes()
                         from attr in attrs
                         where attr is Messgae
                         select field;
            IDictionary<String, Object> map = new Dictionary<String, Object>();
            foreach (var field in fields)
            {
                Messgae message = field.FieldType.GetCustomAttribute(typeof(Messgae)) as Messgae;
                map.Add(message.KeyName, field.GetValue(data));
            }
            return list.Push(map);
        }
    }
}
