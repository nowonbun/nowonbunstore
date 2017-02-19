using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Common
{
    public static class Define
    {
        public static int SELECT_OPTION_YEAR_START = 2014;
        public static int SELECT_OPTION_YEAR_END = 2020;
        public static int SELECT_OPTION_MONTH_START = 1;
        public static int SELECT_OPTION_MONTH_END = 12;
        public static int SELECT_OPTION_DAY_START = 1;
        public static int SELECT_OPTION_DAY_31_END = 31;

        public static String POST_PAGE = "1";

        public static String USER_SESSION_NAME = "USER_SESSION";

        public static String RESULT_OK = "OK";
        public static String RESULT_NG = "NG";
        public static String LOGIN_ERROR = "SIGNERROR";

        public static String PDT_FORMAT = "yyyyMMddHHmmssfff";
    }
}