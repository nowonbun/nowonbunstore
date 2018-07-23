using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;
using Newtonsoft.Json;

namespace WebScraping.Scraper.Flow.Halfclub
{
    partial class HalfclubFlow : AbstractScrapFlow
    {
        private void Login(FlowModelData data)
        {
            data.Document.GetElementById<GeckoInputElement>("PartnerID").Value = Parameter.Id1;
            data.Document.GetElementById<GeckoInputElement>("PartnerPwd").Value = Parameter.Pw1;
            data.Document.GetElementById<GeckoInputElement>("ImgbtnSubmit").Click();
        }
        private void Default(FlowModelData data)
        {
            ExcuteJavascript(data.Document, "$(function(){window.alert = function(){};});");
            //base.Navigate("//scm.halfclub.com/PrivateMng/CompanyModify.aspx");
        }
        private void CompanyModify(FlowModelData data)
        {
        }
    }
}
