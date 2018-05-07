using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PTM.Httpd.Util;

namespace PTM.StartConsole
{
    class WSNode
    {
        public int Type { get; set; }
        public String Key { get; set; }
        public String Data { get; set; }
        public String ResponseKey { get; set; }
        public String ResponseData { get; set; }
        public static WSNode ToNode(String json)
        {
            return JsonConvert.DeserializeObject<WSNode>(json);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public String2 ToString2()
        {
            return new String2(ToString()); 
        }
    }
}
