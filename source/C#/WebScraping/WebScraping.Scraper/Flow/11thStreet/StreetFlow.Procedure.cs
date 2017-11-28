using System;
using System.Collections.Generic;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Other;
using Newtonsoft.Json;


namespace WebScraping.Scraper.Flow._11thStreet
{
    partial class StreetFlow : AbstractScrapFlow
    {
        private Boolean Login(GeckoDocument document, Uri uri)
        {
            if (uri.ToString().IndexOf("returnURL") != -1)
            {
                SetCommonData(0, "FALSE");
                return false;
            }
            document.GetElementById<GeckoInputElement>("loginName").Value = Parameter.Id;
            document.GetElementById<GeckoInputElement>("passWord").Value = Parameter.Pw;
            document.GetElementByClassName<GeckoInputElement>("btn_login").Click();
            logger.Debug("Check Debug");
            return true;
        }
        private Boolean Index(GeckoDocument document, Uri uri)
        {
            SetCommonData(0, "TRUE");
            var data26 = document.GetElementById<GeckoElement>("prdQnaAnswerCnt").FirstChild.NodeValue;
            SetCommonData(26, data26);
            var data25 = document.GetElementById<GeckoElement>("emergencyCnt").FirstChild.NodeValue;
            SetCommonData(25, data25);
            return true;
        }
    }
}
