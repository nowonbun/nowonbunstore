using AisProjectCore.Dinject.Abstract;
using AisProjectCore.Dinject.Concrete;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AisProjectCore.Dinject.Inheritance
{
    /// <summary>
    /// inject dependency object to field in target class
    /// </summary>
    public class Injector
    {
        public Injector()
        {
            Inject();
        }

        public void Inject()
        {
            Inject(this);
        }
        
        /// <summary>
        /// inject dependency object
        /// </summary>
        /// <param name="obj">target class instance</param>
        public static void Inject(object obj)
        {
            //flyweight
            //IKernel kernel = new StandardKernel();
            IKernel kernel = FactoryCore.getKernel();

            // get fields
            Type type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // find interface
            foreach(var field in fields)
            {
                Type field_type = field.FieldType;
                if(field_type.IsInterface)
                {
                    try
                    {
                        // inject
                        object instance = kernel.Get(field_type);
                        field.SetValue(obj, instance);
                    }
                    catch (KeyNotFoundException)
                    {
                        //log
                        // not found interface key.
                    }
                }
            }
        }
    }
}
