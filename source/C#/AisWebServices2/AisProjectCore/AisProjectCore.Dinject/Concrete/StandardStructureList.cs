using System;
using System.Collections.Generic;
using AisProjectCore.Dinject.Abstract;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;

namespace AisProjectCore.Dinject.Concrete
{
    class StandardStructureList : IStructureList
    {
        private IDictionary<Node, String> map = new Dictionary<Node, String>();
        struct Node
        {
            public String key;
            public DateTime time;
        }

        public StandardStructureList()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    lock (map)
                    {
                        var keylist = (from key in map.Keys where key.time > DateTime.Now.AddHours(-1) select key).ToList();
                        foreach (var key in keylist)
                        {
                            map.Remove(key);
                        }
                    }
                    //1hour
                    Thread.Sleep(1000 * 60 * 60);
                }
            });
        }

        public String Push(IDictionary<String, Object> data)
        {
            lock (map)
            {
                Node nodekey;
                nodekey.key = "AIS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                nodekey.time = DateTime.Now;
                map.Add(nodekey, JsonConvert.SerializeObject(data));
                return nodekey.key;
            }
        }
        public String Push(IList<IDictionary<String, Object>> data)
        {
            lock (map)
            {
                Node nodekey;
                nodekey.key = "AIS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                nodekey.time = DateTime.Now;
                map.Add(nodekey, JsonConvert.SerializeObject(data));
                return nodekey.key;
            }
        }
        public String Pop(String key)
        {
            lock (map)
            {
                var mapkey = (from nodekey in map.Keys where String.Equals(nodekey.key, key) select nodekey);
                if (mapkey.Count() == 1)
                {
                    String ret = map[mapkey.Single()];
                    map.Remove(mapkey.Single());
                    return ret;
                }
                return null;
            }
        }
    }
}
