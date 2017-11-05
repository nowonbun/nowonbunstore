using System;

namespace LogisticsSystem.App_Code
{
    public class Define
    {
        public const String MASTER_VIEW = "~/Views/Master.cshtml";
        public const String POPUP_VIEW = "~/Views/PopupMaster.cshtml";

        public const int STATE_NORMAL = 0;
        public const int STATE_DELETE = 1;
        public const int STATE_APPLY = 2;

        public const String NOTDEL = "0";
        public const String DEL = "1";

        public const int PAGE_START = 1;

        public const String APPROVE = "approve";
        public const String CANCEL = "cancel";

        public class Session
        {
            public const String CONTROLLER = "controller";
            public const String ACTION = "action";
            public const String SESSION_CHECK = "SessionCheck";
            public const String BROWSER_CHECK = "BrowserCheck";
            public const String AJAX_CHECK = "AjaxCheck";
            public const String AUTH_CHECK = "AuthCheck";
            public const String MASTER = "master";
            public const String SESSION_ID = "SessionID";
            public const String ERROR_MESSAGE = "ErrorMsg";
            public const String ID = "Identity";

            public const String USER_INFO = "userinfo";
            public const String COMPANY_INFO = "compinfo";
            public const String LANGUAGE_TYPE = "languageType";
            public const String IMAGE = "imageBuffer";

            public const String VIEW_NAME = "viewName";
            public const String FORM_LANG = "Lang";
            public const String FORM_KOREA = "k";
            public const String FORM_JAPAN = "j";
        }
        public class CodeMaster
        {
            public const String CUSTOMER_TYPE = "customerType";
            public const String MONEY_SEND_TYPE = "MoneySendType";
            public const String CUSTOMER_TAX_TYPE = "customerTaxType";
            public const String PRODUCT_TYPE = "productType";
            public const String PRODUCT_SPEC = "productSpec";
        }
        public class ProductFlow
        {
            public const int INCOMESTANBY = 1;
            public const int INCOMECOMPLATE = 2;
            public const int OUTCOMESTANBY = 3;
            public const int OUTPUTCOMPLATE = 4;
            public const int INCOMECANCEL = 5;
            public const int OUTPUTCANCEL = 6;
            public const Int64 APPLYTYPE_NORMAL = 0;
        }
    }
}