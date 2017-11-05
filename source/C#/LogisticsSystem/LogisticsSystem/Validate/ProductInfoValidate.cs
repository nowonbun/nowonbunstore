using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class ProductInfoValidate
    {
        public static List<String> Validate(this ProductInfo entity, LanguageType? lType)
        {
            List<String> Errmsg = new List<string>();
            if (lType == LanguageType.Korea)
            {
                if (entity.ProductCode == null || entity.ProductCode.Equals(""))
                {
                    Errmsg.Add("상품코드가 입력되지 않았습니다.");
                }
                if (entity.ProductName == null || entity.ProductName.Equals(""))
                {
                    Errmsg.Add("상품이름이 입력되지 않았습니다.");
                }
            }
            else
            {
                if (entity.ProductCode == null || entity.ProductCode.Equals(""))
                {
                    Errmsg.Add("商品コードが入力されてありません。");
                }
                if (entity.ProductName == null || entity.ProductName.Equals(""))
                {
                    Errmsg.Add("商品名が入力されてありません。");
                }
            }
            return Errmsg;
        }
    }
}