using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Household.Models.Master;
using Household.Models.Entity;

namespace Household.Common
{
    public static class HtmlUtil
    {
        private static String clientID = null;
        private static String clientSecret = null;
        private static String redirectUrl = null;

        public static void Initialize(String clientID,String clientSecret,String redirectUrl)
        {
            HtmlUtil.clientID = clientID;
            HtmlUtil.clientSecret = clientSecret;
            HtmlUtil.redirectUrl = redirectUrl;
        }

        public static String GetClientID()
        {
            return HtmlUtil.clientID;
        }
        public static String GetClientSecret()
        {
            return HtmlUtil.clientSecret;
        }
        public static String GetRedirectUrl()
        {
            return HtmlUtil.redirectUrl;
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
                buffer.Append("' >"); ;
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
                buffer.Append("' >"); ;
                buffer.Append(i);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }

        public static String GetCategoryOption()
        {
            StringBuilder buffer = new StringBuilder();
            CategoryMaster list = FactoryMaster.Instance().GetCategoryMaster();
            foreach (Category category in list)
            {
                buffer.Append("<option value='");
                buffer.Append(category.Cd);
                buffer.Append("' >"); ;
                buffer.Append(category.Nm);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }

        public static String GetTypeTemplateOption()
        {
            StringBuilder buffer = new StringBuilder();
            CategoryMaster master = FactoryMaster.Instance().GetCategoryMaster();
            foreach (Category l in master)
            {
                buffer.Append("<select id='select_");
                buffer.Append(l.Cd);
                buffer.Append("' >");
                var tplist = FactoryMaster.Instance().GetTypeMaster().GetByCategory(l.Cd);
                foreach (Household.Models.Entity.Type t in tplist)
                {
                    buffer.Append("<option value='");
                    buffer.Append(t.Tp);
                    buffer.Append("'>");
                    buffer.Append(t.Nm);
                    buffer.Append("</option>");
                }
                buffer.Append("</select>");
            }
            return buffer.ToString();
        }

        public static String GetSearchOption()
        {
            StringBuilder buffer = new StringBuilder();
            var list = FactoryMaster.Instance().GetTypeMaster().GetBySearchCode();
            foreach (var l in list)
            {
                buffer.Append("<option value='");
                buffer.Append(l.Tp);
                buffer.Append("'>");
                buffer.Append(l.Nm);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }
        public static String GetConfig(String id)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}