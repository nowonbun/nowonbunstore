using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class CompanyInfoValidate
    {
        public static IList<String> Validate(this CompanyInfo entity, LanguageType? lType)
        {
            IList<String> Errmsg = new List<string>();
            if (StaticUtil.NullCheck(entity.CompanyName))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("회사이름이 입력되지 않았습니다.");
                else Errmsg.Add("会社名が入力されてありません。");
            }
            if ((StaticUtil.NullCheck(entity.CompanyPostNumber1) || StaticUtil.NullCheck(entity.CompanyPostNumber2)))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("우편번호가 입력되지 않았습니다.");
                else Errmsg.Add("ポスト番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanyAddress))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("주소가 입력되지 않았습니다.");
                else Errmsg.Add("住所が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanySecurityNumber))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("사업자번호가 입력되지 않았습니다.");
                else Errmsg.Add("事業者番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanySecurityNumber))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("사업자번호가 입력되지 않았습니다.");
                else Errmsg.Add("事業者番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanyNumber1) || StaticUtil.NullCheck(entity.CompanyNumber2) || StaticUtil.NullCheck(entity.CompanyNumber3))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("회사전화번호가 입력되지 않았습니다.");
                else Errmsg.Add("会社電話番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanyEmail))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("회사이메일이 입력되지 않았습니다.");
                else Errmsg.Add("会社Eメールが入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.Representative))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("대표자명이 입력되지 않았습니다.");
                else Errmsg.Add("代表者名が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.RepresentativeNumber1) || StaticUtil.NullCheck(entity.RepresentativeNumber2) || StaticUtil.NullCheck(entity.RepresentativeNumber3))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("대표자전화번호가 입력되지 않았습니다.");
                else Errmsg.Add("代表者電話番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.RepresentativeEmail))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("대표자이메일이 입력되지 않았습니다.");
                else Errmsg.Add("代表者Eメールが入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanyAccountBank))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("계좌은행명이 입력되지 않았습니다.");
                else Errmsg.Add("通帳銀行名が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanyAccountOwnerName))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("예금주가 입력되지 않았습니다.");
                else Errmsg.Add("通帳取付が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.CompanyAccountNumber))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("계좌번호가 입력되지 않았습니다.");
                else Errmsg.Add("通帳番号名が入力されてありません。");
            }

            return Errmsg;
        }
    }
}