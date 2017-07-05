using AisProjectCore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisProjectCore.Dinject.Concrete;

namespace AisProjectCore.Domain.Concrete
{
    public class TestProc : ITestProc
    {
        class Node
        {
            String Result;
            String Data;
            String Error;
            public IDictionary<String, Object> Build()
            {
                IDictionary<String, Object> ret = new Dictionary<String, Object>();
                ret.Add("Result",Result);
                return ret;
            }
        }
        public string HelloWorld()
        {
            //Result // Data // Error
            IDictionary<String, Object> ret = new Dictionary<String, Object>();
            ret.Add("Result", 0);
            ret.Add("Error", null);
            ret.Add("Data", "Helloworld");
            return FactoryCore.getList().Push(ret);
        }

        public string Print(string param01, string param02)
        {
            return $"{param01}, {param02}";
        }
    }
}
