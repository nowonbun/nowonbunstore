using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class OrderTableSubValidate
    {
        public static List<String> Validate(this OrderTableSub entity, LanguageType? lType, int i)
        {
            List<String> Errmsg = new List<string>();
            if (entity.ProductIndex <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add(i.ToString() + "번째 상품번호가 입력되지 않았습니다..");
                else Errmsg.Add(i.ToString() + "番目、商品番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.ProductSpec))
            {
                if (lType == LanguageType.Korea) Errmsg.Add(i.ToString() + "번째 상품규격이 입력되지 않았습니다..");
                else Errmsg.Add(i.ToString() + "番目、商品規格が入力されてありません。");
            }
            if (entity.ProductAmount <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add(i.ToString() + "번째 상품양이 입력되지 않았습니다..");
                else Errmsg.Add(i.ToString() + "番目、商品量が入力されてありません。");
            }
            if (entity.ProductPrice <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add(i.ToString() + "번째 상품가격이 입력되지 않았습니다..");
                else Errmsg.Add(i.ToString() + "番目、商品価格が入力されてありません。");
            }
            if (entity.ProductMoney <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add(i.ToString() + "번째 금액이 입력되지 않았습니다..");
                else Errmsg.Add(i.ToString() + "番目、金額が入力されてありません。");
            }
            return Errmsg;
        }
    }
}