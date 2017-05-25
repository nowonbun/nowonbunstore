using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Common
{
    public static class Message
    {
        public static String LOGIN_ERROR_NOT_GROUPID = "Enter the group id,Please";
        public static String LOGIN_ERROR_NOT_ID = "Enter the id,Please";
        public static String LOGIN_ERROR_NOT_PWD = "Enter the password,Please";
        public static String LOGIN_ERROR = "Invalid password. Please try again.";

        public static String DATA_EROR = "不正的にデータ入力をできません。";
        public static String DATA_CHECK = "他のユーザがデータを修正しました。";
        public static String EXCEPTION = "エラー発生しました。管理者にお問合せください。";
    }
}