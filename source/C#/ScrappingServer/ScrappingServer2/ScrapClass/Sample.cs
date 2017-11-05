using System;
using System.Text;
using System.IO;
using ScrappingCore;
using log4net;
using log4net.Config;
using ScrappingServer.Dao;
using ScrappingServer.Entity;
using System.Collections.Generic;

namespace ScrappingServer.ScrapClass
{
    public class Sample : AbstractClass,ICommonScrap
    {
        public Sample(string id, string pw, string[] args)
            : base(id, pw, args)
        {

        }
        protected override void Initialize()
        {

        }

        protected override void Execute(IScrapping scrap)
        {
            //TODO : http://localhost:10000/?type=scrap_request&code=999&apply=1&id=TEST&pw=TEST
        }
        protected override void Finish()
        {
            InfoLog("TEST");
            InfoLog("apply = " + PARAM[0]);
            InfoLog("id = " + ID);
            InfoLog("pw = " + PW);
            SetEventHandler(ScrapState.COMPLETE);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
        protected override void Navigated(string url)
        {
            DebugLog("Navigated : " + url);
        }
        protected override void Error(Exception e)
        {
            ErrorLog(e);
            SetEventHandler(ScrapState.ERROR);
        }
    }
}