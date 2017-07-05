using System;
using System.Collections.Generic;
using AisProjectCore.Dinject.Abstract;

namespace AisProjectCore.Dinject.Concrete
{
    public static class FactoryCore
    {
        private static IDictionary<Type, object> flyweight = new Dictionary<Type, object>();
        public static IKernel getKernel()
        {
            if (!flyweight.ContainsKey(typeof(IKernel)))
            {
                flyweight.Add(typeof(IKernel), new StandardKernel());
            }
            return flyweight[typeof(IKernel)] as IKernel;
        }
        public static ISocket getSocket()
        {
            if (!flyweight.ContainsKey(typeof(ISocket)))
            {
                //The next port should defined necessary what is property.
                flyweight.Add(typeof(ISocket), new StandardSocket(15000));
            }
            return flyweight[typeof(ISocket)] as ISocket;
        }
        public static IStructureList getList()
        {
            if (!flyweight.ContainsKey(typeof(IStructureList)))
            {
                //The next port should defined necessary what is property.
                flyweight.Add(typeof(IStructureList), new StandardStructureList());
            }
            return flyweight[typeof(IStructureList)] as IStructureList;
        }
    }
}
