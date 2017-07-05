using AisProjectCore.Dinject.Abstract;
using System;
using System.Collections.Generic;

namespace AisProjectCore.Dinject.Concrete
{
    class StandardKernel : IKernel
    {
        // di policy dictionary
        // 私はstaticを好きじゃない。。。涙
        private static IDictionary<Type, Type> DI_Policies = new Dictionary<Type, Type>();

        // find di policy
        private Type FindPolicy(Type policy_key)
        {
            Type ty;
            if(!DI_Policies.TryGetValue(policy_key, out ty))
            {
                throw new KeyNotFoundException("unknown interface type : " + policy_key.Name);
            }

            return ty;
        }

        // binding di policy
        public IKernel Binding<T1, T2>()
        {
            DI_Policies.Add(typeof(T1), typeof(T2));
            return this;
        }

        // instance object from policy
        public object Get<T>()
        {
            return Get(typeof(T));
        }

        public object Get(Type t)
        {
            return Activator.CreateInstance(FindPolicy(t));
        }

        //not reference??
        //public object Instance(Type t)
        //{
        //    // Parameter list to ctor
        //    IList<Object> parameters = new List<Object>();

        //    // get ctor list in service type t
        //    var ctor_type = t.GetConstructors();
        //    ConstructorInfo ctorinfo = null;

        //    // find ctor
        //    foreach (var ctor in ctor_type)
        //    {
        //        var parms = ctor.GetParameters();
        //        foreach (var param in parms)
        //        {
        //            Type paramtype = param.ParameterType;

        //            // check interface type of parameter
        //            if (paramtype.IsInterface)
        //            {
        //                // added parameter
        //                ctorinfo = ctor;
        //                object prm = Get(paramtype);
        //                parameters.Add(prm);
        //            }
        //        }

        //        // found ctor
        //        if (ctorinfo != null)
        //            break;
        //    }

        //    if (ctorinfo == null)   // not found
        //        return null;

        //    // create instance
        //    return ctorinfo.Invoke(parameters.ToArray());
        //}
    }
}
