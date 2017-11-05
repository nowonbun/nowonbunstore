using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class CustomerInfoValidate
    {
        /// <summary>
        /// Validate
        /// </summary>
        public static List<String> Validate(this CustomerInfo entity,LanguageType? lType)
        {
            List<String> Errmsg = new List<string>();
            if (lType == LanguageType.Korea)
            {
                if (entity.CustomerCode == null || entity.CustomerCode.Equals(""))
                {
                    Errmsg.Add("상품코드가 입력되지 않았습니다.");
                }
                if (entity.CustomerName == null || entity.CustomerName.Equals(""))
                {
                    Errmsg.Add("상품이름이 입력되지 않았습니다.");
                }
                if (entity.CustomerRepresetitive == null || entity.CustomerRepresetitive.Equals(""))
                {
                    Errmsg.Add("대표자명이 입력되지 않았습니다.");
                }
                if (entity.CustomerNumber == null || entity.CustomerNumber.Equals(""))
                {
                    Errmsg.Add("전화번호가 입력되지 않았습니다.");
                }
                if (entity.CustomerFax == null || entity.CustomerFax.Equals(""))
                {
                    Errmsg.Add("팩스번호가 입력되지 않았습니다.");
                }
                if (entity.CustomerPostNumber1 == null || entity.CustomerPostNumber1.Equals(""))
                {
                    Errmsg.Add("우편번호가 입력되지 않았습니다.");
                }
                if (entity.CustomerPostNumber2 == null || entity.CustomerPostNumber2.Equals(""))
                {
                    Errmsg.Add("우편번호가 입력되지 않았습니다.");
                }
                if (entity.CustomerAddress == null || entity.CustomerAddress.Equals(""))
                {
                    Errmsg.Add("주소가 입력되지 않았습니다.");
                }
            }
            else
            {
                if (entity.CustomerCode == null || entity.CustomerCode.Equals(""))
                {
                    Errmsg.Add("商品コードが入力されてありません。");
                }
                if (entity.CustomerName == null || entity.CustomerName.Equals(""))
                {
                    Errmsg.Add("商品名が入力されてありません。");
                }
                if (entity.CustomerRepresetitive == null || entity.CustomerRepresetitive.Equals(""))
                {
                    Errmsg.Add("代表者名が入力されてありません。");
                }
                if (entity.CustomerNumber == null || entity.CustomerNumber.Equals(""))
                {
                    Errmsg.Add("電話番号が入力されてありません。");
                }
                if (entity.CustomerFax == null || entity.CustomerFax.Equals(""))
                {
                    Errmsg.Add("ファクス番号が入力されてありません。");
                }
                if (entity.CustomerPostNumber1 == null || entity.CustomerPostNumber1.Equals(""))
                {
                    Errmsg.Add("ポスト番号が入力されてありません。");
                }
                if (entity.CustomerPostNumber2 == null || entity.CustomerPostNumber2.Equals(""))
                {
                    Errmsg.Add("ポスト番号が入力されてありません。");
                }
                if (entity.CustomerAddress == null || entity.CustomerAddress.Equals(""))
                {
                    Errmsg.Add("住所が入力されてありません。");
                }
            }
            return Errmsg;
        }
    }
}