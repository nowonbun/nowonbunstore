using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class ProductFlowValidate
    {
        public static List<String> Validate(this ProductFlow entity,LanguageType? lType)
        {
            List<String> Errmsg = new List<string>();
            if (entity.ProductIndex <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add("상품이 선택되지 않았습니다.");
                else Errmsg.Add("商品が選択されてありません。。");
            }
            if (entity.ProductAmount <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add("상품수량이 입력되지 않았습니다.");
                else Errmsg.Add("商品数量が入力されてありません。");
            }
            return Errmsg;
        }
    }
}