using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Household.Common
{
    public static class HtmlUtil
    {
        private static String clientID = "";
        private static String clientSecret = "";

        public static String GetClientID()
        {
            return HtmlUtil.clientID;
        }
        public static String GetClientSecret()
        {
            return HtmlUtil.clientSecret;
        }

        public static String GetSelectYearOption()
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = Define.SELECT_OPTION_YEAR_START; i <= Define.SELECT_OPTION_YEAR_END; i++)
            {
                buffer.Append("<option value='");
                buffer.Append(i);
                buffer.Append("' >");
                buffer.Append(i);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }

        public static String GetSelectMonthOption()
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = Define.SELECT_OPTION_MONTH_START; i <= Define.SELECT_OPTION_MONTH_END; i++)
            {
                buffer.Append("<option value='");
                buffer.Append(i);
                buffer.Append("' >");;
                buffer.Append(i);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }

        public static String GetSelectDay31Option()
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = Define.SELECT_OPTION_DAY_START; i <= Define.SELECT_OPTION_DAY_31_END; i++)
            {
                buffer.Append("<option value='");
                buffer.Append(i);
                buffer.Append("' >");;
                buffer.Append(i);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }

        public static String GetCategoryOption()
        {
            StringBuilder buffer = new StringBuilder();

            return buffer.ToString();
        }

        public static String GetTypeTemplateOption()
        {
            StringBuilder buffer = new StringBuilder();

            return buffer.ToString();
        }

        public static String GetSearchOption()
        {
            StringBuilder buffer = new StringBuilder();

            return buffer.ToString();
        }
    }
}