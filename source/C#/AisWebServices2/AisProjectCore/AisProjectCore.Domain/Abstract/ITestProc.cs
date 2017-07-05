using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Abstract
{
    /// <summary>
    /// test sample interface
    /// </summary>
    public interface ITestProc
    {
        /// <summary>
        /// print "Hello World!"
        /// </summary>
        /// <returns>"Hello World!" string</returns>
        string HelloWorld();

        /// <summary>
        /// print "param01, param02"
        /// </summary>
        /// <param name="param01">param 1</param>
        /// <param name="param02">param 2</param>
        /// <returns>"param01, param02" string</returns>
        string Print(string param01, string param02);
    }
}
