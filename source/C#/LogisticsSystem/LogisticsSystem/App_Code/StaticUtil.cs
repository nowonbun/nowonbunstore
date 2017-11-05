using System;

namespace LogisticsSystem.App_Code
{
    public static class StaticUtil
    {
        public static bool NullCheck(String obj)
        {
            if (obj == null || obj.Trim().Equals(String.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}