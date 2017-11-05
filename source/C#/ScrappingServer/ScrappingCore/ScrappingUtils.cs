using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrappingCore
{
    /// <summary>
    /// 스크립트 상태
    /// </summary>
    public enum ScrapState
    {
        INIT = 1,       // 초기화
        RUNNING = 2,    // 스크래핑 중
        COMPLETE = 3,   // 스크래핑 완료
        ERROR = -1,     // 스크래핑 에러
        EMPTY = -2      // 스크래핑 null
    }
    public static class ScrappingUtils
    {
        public static bool ContainOf(String target, String value)
        {
            if (String.IsNullOrEmpty(target))
            {
                return false;
            }
            if (target.IndexOf(value) == -1)
            {
                return false;
            }
            return true;
        }
    }
}
