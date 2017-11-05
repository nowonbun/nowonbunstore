function commander(pstep,link)
{
    var url = "";
    switch(pstep)
    {
        case 1:
            switch(link)
            {
                case 1: url = "/Product/Main"; break;                   //상품등록
                case 2: url = "/Customer/Main"; break;                  //고객등록
                case 3: url = "/Web/CategoryAdmin"; break;              //카테고리설정
                case 4: url = "/Web/Logout"; break;                     //로그아웃
                case 5: url = "/Web/CompanyInfo"; break;                //회사설정
                case 6: url = "/Web/UserInfo"; break;                //회원정보
            }
            break;
        case 2:
            switch (link) {
                case 1: url = "/Obtain/Order"; break;                   //발주신청
                case 2: url = "/Obtain/OrderApproveList"; break;        //발주승인
                case 3: url = "/Obtain/OrderList"; break;              //발주리스트
            }
            break;
        case 3:
            switch (link) {
                case 2: url = "/Store/ApplyAdd"; break;                 //입고등록
                case 1: url = "/Store/ApplyCheckList"; break;           //입고승인리스트
                case 3: url = "/Store/ApplyList"; break;                //입고리스트
            }
            break;
        case 4:
            switch (link) {
                case 4: url = "/Delivery/DeliveryOrder"; break;         //수주신청
                case 1: url = "/Delivery/DeliveryApproveList"; break;    //수주승인
                case 5: url = "/Delivery/DeliveryOrderList"; break;     //수주리스트
                case 2: url = "/Delivery/DeliveryCheckList"; break;     //납품확인서
                case 3: url = "/Delivery/DeliveryBillList"; break;      //세금계산서
            }
            break;
        case 5:
            switch (link) {
                case 2: url = "/Store/ReleaseAdd"; break;               //출고등록
                case 1: url = "/Store/ReleaseCheckList"; break;              //출고승인
                case 3: url = "/Store/ReleaseList"; break;         //출고리스트
            }
            break;
        case 6:
            switch (link) {
                case 1: url = "/Store/StoreList"; break;                     //재고
                case 2: url = "/Store/StoreOrder"; break;               //재고입수불
            }
            break;
        case 7:
            break;
        case 8:
            switch (link) {
                case 1: url = "/Board/List"; break;                     //게시판
                case 2: url = "/Web/ConnectList"; break;                     //게시판
            }
            break;
    }
    //location.href = url;
    document.menuPost.action = url;
    document.menuPost.submit();
}