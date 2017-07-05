using System;

namespace AisProjectCore.Dinject.Abstract
{
    public interface IKernel
    {
        /// <summary>
        /// Add new dependency-injection policy
        /// </summary>
        /// <typeparam name="T1">interface class</typeparam>
        /// <typeparam name="T2">real object class</typeparam>
        /// <returns>kernel object</returns>
        IKernel Binding<T1, T2>();

        /// <summary>
        /// instance object with DI-Policy
        /// </summary>
        /// <typeparam name="T">interface class</typeparam>
        /// <returns>instance</returns>
        object Get<T>();

        /// <summary>
        /// instance object with DI-Policy
        /// </summary>
        /// <param name="t">interface class</param>
        /// <returns>instance</returns>
        object Get(Type t);

        /// <summary>
        /// not reference
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        //object Instance(Type t);
    }
}
