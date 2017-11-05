using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;

namespace LogisticsSystem.Validate
{
    public static class OrderTableValidate
    {
        public static List<String> Validate(this OrderTable entity,LanguageType? lType)
        {
            List<String> Errmsg = new List<string>();
            if (StaticUtil.NullCheck(entity.OrderNumber))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("문서번호가 입력되지 않았습니다..");
                else Errmsg.Add("文書番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.OrderName))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("발주사가 입력되지 않았습니다..");
                else Errmsg.Add("発注社が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.OrderAddress))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("발주사주소가 입력되지 않았습니다..");
                else Errmsg.Add("発注社住所が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.OrderPhoneNumber))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("발주사전화번호가 입력되지 않았습니다..");
                else Errmsg.Add("発注社電話番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.OrderFax))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("발주사팩스번호가 입력되지 않았습니다..");
                else Errmsg.Add("発注社ファクス番号が入力されてありません。");
            }
            if (StaticUtil.NullCheck(entity.InorderName))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("수주자이름가 입력되지 않았습니다..");
                else Errmsg.Add("受注社が入力されてありません。");
            }
            if (entity.OrderMoney <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add("발주금액이 없습니다.");
                else Errmsg.Add("発注金額がありません。");
            }
            if (entity.OrderSaveDate == null || entity.OrderSaveDate < DateTime.Now.AddDays(-1))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("납기일자가 맞지 않습니다.");
                else Errmsg.Add("納期日付が合わないです。");
                if (entity.OrderSaveDate < DateTime.Now.AddYears(-1))
                {
                    entity.OrderSaveDate = DateTime.Now;
                }
            }
            if (StaticUtil.NullCheck(entity.OrderSavePlace))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("납기장소가 입력되지 않았습니다..");
                else Errmsg.Add("納期場所が入力されてありません。");
            }
            if (entity.OrderDate == null || entity.OrderDate < DateTime.Now.AddDays(-1))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("발주일자가 맞지 않습니다.");
                else Errmsg.Add("発注日付が合わないです。");
                if (entity.OrderDate < DateTime.Now.AddYears(-1))
                {
                    entity.OrderDate = DateTime.Now;
                }
            }
            if (entity.PayDate == null || entity.PayDate < DateTime.Now.AddDays(-1))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("지불일자가 맞지 않습니다.");
                else Errmsg.Add("支払日付が合わないです。");
                if (entity.PayDate < DateTime.Now.AddYears(-1))
                {
                    entity.PayDate = DateTime.Now;
                }
            }
            if (entity.PayMoney <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add("지불금액이 없습니다.");
                else Errmsg.Add("支払金額がありません。");
            }
            if (StaticUtil.NullCheck(entity.PayCondition))
            {
                if (lType == LanguageType.Korea) Errmsg.Add("지불조건이 선택되지 않았습니다..");
                else Errmsg.Add("支払条件が選択されてありません。");
            }
            return Errmsg;
        }
    }
}