using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Household.Models.Master;
using HouseholdORM;

namespace Household.Common
{
    public static class HtmlUtil
    {
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
            IList<Ctgry> list = FactoryMaster.Instance().GetCategoryMaster().GetAll();
            foreach(Ctgry l in list){
                buffer.Append("<option value='");
                buffer.Append(l.Cd);
                buffer.Append("' >");;
                buffer.Append(l.Nm);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }

        public static String GetTypeTemplateOption()
        {
            StringBuilder buffer = new StringBuilder();
            IList<Ctgry> list = FactoryMaster.Instance().GetCategoryMaster().GetAll();
            foreach (Ctgry l in list)
            {
                buffer.Append("<select id='select_");
                buffer.Append(l.Cd);
                buffer.Append("' >");
                IList<Tp> tplist = FactoryMaster.Instance().GetTypeMaster().GetByCategoryCode(l.Cd);
                foreach(Tp t in tplist){
                    buffer.Append("<option value='");
                    buffer.Append(t.TP);
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
            IList<Tp> list = FactoryMaster.Instance().GetTypeMaster().GetSearchCode();
            foreach (Tp l in list)
            {
                buffer.Append("<option value='");
                buffer.Append(l.TP);
                buffer.Append("'>");
                buffer.Append(l.Nm);
                buffer.Append("</option>");
            }
            return buffer.ToString();
        }
    }
}