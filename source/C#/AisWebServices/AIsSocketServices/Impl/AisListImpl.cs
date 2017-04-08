using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AIsSocketServices.Impl
{
    class AisListImpl : IAisList
    {
        private IDictionary<String,String> map = new Dictionary<String,String>();
        public String Push(IDictionary<String,Object> data)
        {
            String key = "AIS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            map.Add(key, JsonConvert.SerializeObject(data));
            return key;
        }
        public String Push(IList<IDictionary<String, Object>> data)
        {
            String key = "AIS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            map.Add(key, JsonConvert.SerializeObject(data));
            return key;
        }
        public String Pop(String key)
        {
            if (map.ContainsKey(key))
            {
                String ret = map[key];
                map.Remove(key);
                return ret;
            }
            return null;
        }
        
    }
}
