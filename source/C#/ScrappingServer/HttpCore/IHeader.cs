using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrappingHttpCore
{
    public interface IHeader
    {
        /// <summary>
        /// 파라미터 (맵형식으로 구성되어 있다(
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        String this[String parameterName] { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool GetState();
    }
}
