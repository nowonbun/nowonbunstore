using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class UserInfoValidate
    {
        public static IList<String> Validate(this UserInfo entity, LanguageType? lType)
        {
            IList<String> Errmsg = new List<string>();
            if (StaticUtil.NullCheck(entity.UserName))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("유저명이 입력되지 않았습니다.");
                else Errmsg.Add("名前が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.UserNumber1) || StaticUtil.NullCheck(entity.UserNumber2) || StaticUtil.NullCheck(entity.UserNumber3))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("전화번호가 입력되지 않았습니다.");
                else Errmsg.Add("電話番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.UserEmail))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("이메일이 입력되지 않았습니다.");
                else Errmsg.Add("Eメールが入力されてありません。");
            }
            if (!StaticUtil.NullCheck(entity.Password))
            {
                if (!entity.Password.Equals(entity.PasswordCheck))
                {
                    if (lType == LanguageType.Korea) Errmsg.Add("패스워드가 일치하지 않습니다.");
                    else Errmsg.Add("パスワードが一致しません。");
                }
            }
            return Errmsg;
        }
    }
}