using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class DocumentValidate
    {
        public static List<String> Validate(this Document entity, LanguageType? lType)
        {
            List<String> Errmsg = new List<string>();
            if (StaticUtil.NullCheck(entity.DocumentCode))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("문서번호가 입력되지 않았습니다..");
                else Errmsg.Add("文書番号が入力されてありません。");
            }
            return Errmsg;
        }
    }
}