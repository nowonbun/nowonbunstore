using System;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class ReturnRequest
    {
        [ExcelHeader("구매자명*", 0)]
        private String buyerName;

        public String BuyerName
        {
            get { return this.buyerName; }
        }
        //구매자명*	수령인명*	아이디*	주문번호*	주문일자(결제확인전)*	반품신청일*	수거완료일*	환불보류일*	환불승인일*	상품번호*	상품명*	판매금액*	수량*	필수선택*	추가구성*	배송번호*	배송비*	배송비금액	반품접수채널*	반품사유*	상세사유*	상품상태*	최초배송비부담*	반품수거택배사명*	반품송장번호*	반품배송비지불주체*	반품배송비금액*	반품추가비금액*	반품추가비지불주체*	구매자ID	구매자 휴대폰	구매자 전화번호	수령인 휴대폰	수령인 전화번호	환불보류해제일*	원배송택배사명*	원배송송장번호*	환불보류사유*	장바구니번호(결제번호)*	판매자관리코드*	판매자상세관리코드*

    }
}
