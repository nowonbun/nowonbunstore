using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AisProjectCore.Service.Service
{
    public class JsonStructure
    {
        private int result;
        private string error;
        private object data;
        public int Result
        {
            get { return this.result; }
            set { this.result = value; }
        }
        public string Error
        {
            get { return this.error; }
            set { this.error = value; }
        }
        public object Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
        public IDictionary<string, object> build()
        {
            IDictionary<String, object> ret = new Dictionary<String, object>();
            ret.Add("Result", result);
            ret.Add("Error", error);
            ret.Add("Data", data);
            return ret;
        }
    }
}