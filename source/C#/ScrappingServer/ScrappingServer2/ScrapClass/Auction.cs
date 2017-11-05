using System;
using System.Text;
using System.IO;
using ScrappingCore;
using log4net;
using log4net.Config;
using ScrappingServer.Dao;
using ScrappingServer.Entity;
using System.Collections.Generic;
using mshtml;

namespace ScrappingServer.ScrapClass
{
    /// <summary>
    /// Auther : SoonYub Hwang
    /// Date : 27. 11. 2016
    /// This is System to script Auction
    /// </summary>
    public class Auction : AbstractClass, ICommonScrap
    {
        public Auction(string id, string pw, string[] args)
            : base(id, pw, args)
        {

        }
        protected override void Initialize()
        {

        }

        protected override void Execute(IScrapping scrap)
        {
            try
            {
                //http://localhost:10000/?type=scrap_request&code=006&apply=2&id=wogjsl0213&pw=dhwogjs02!
                //It deny the tag to speed up scrapping.
                scrap.AddPrintDenyTagName("META");
                scrap.AddPrintDenyTagName("SCRIPT");
                scrap.AddPrintDenyTagName("LINK");
                scrap.AddPrintDenyTagName("NOSCRIPT");
                scrap.AddPrintDenyTagName("STYLE");

                scrap.Move("https://www.esmplus.com/Member/SignIn/LogOn");

                scrap.ClickByxPath("HTML[0]/BODY[1]/DIV[0]/DIV[2]/DIV[0]/DIV[0]/DIV[1]/DIV[0]/DIV[4]/DIV[0]/DIV[0]/DIV[0]/DIV[1]/LABEL[1]/INPUT[0]");
                scrap.SetInputValueByxPath("HTML[0]/BODY[1]/DIV[0]/DIV[2]/DIV[0]/DIV[0]/DIV[1]/DIV[0]/DIV[4]/DIV[0]/DIV[0]/DIV[0]/FORM[3]/FIELDSET[4]/DIV[1]/INPUT[0]", ID);
                scrap.SetInputValueByxPath("HTML[0]/BODY[1]/DIV[0]/DIV[2]/DIV[0]/DIV[0]/DIV[1]/DIV[0]/DIV[4]/DIV[0]/DIV[0]/DIV[0]/FORM[3]/FIELDSET[4]/DIV[1]/INPUT[1]", PW);
                IHTMLElement element = scrap.GetElementByXPath("HTML[0]/BODY[1]/DIV[0]/DIV[2]/DIV[0]/DIV[0]/DIV[1]/DIV[0]/DIV[4]/DIV[0]/DIV[0]/DIV[0]/FORM[3]/FIELDSET[4]/DIV[2]/A[1]/IMG[0]");
                element.outerHTML = "<input type=button onclick='onSubmit(\"SITE\");' value='go'>";
                scrap.SetDocumentCount(3);
                scrap.ClickByxPath("HTML[0]/BODY[1]/DIV[0]/DIV[2]/DIV[0]/DIV[0]/DIV[1]/DIV[0]/DIV[4]/DIV[0]/DIV[0]/DIV[0]/FORM[3]/FIELDSET[4]/DIV[2]/A[1]/INPUT[0]");
                String urlBuffer = scrap.GetUrl();
                if (urlBuffer.IndexOf("LogOn") > 0)
                {
                    SetData(0, false);
                    return;
                }
                SetData(0, true);
                scrap.Move("https://www.esmplus.com/Home/SSO?code=TDM155");
                Console.WriteLine(scrap.GetUrl());

                SetData(1, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[0]/TD[1]/SPAN[0]/SPAN[0]"));
                SetData(2, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[11]/TD[1]/SPAN[0]"));
                String email = scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[18]/TD[1]/DIV[0]/DIV[0]/INPUT[0]");
                email += "@";
                email += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[18]/TD[1]/DIV[0]/DIV[2]/INPUT[0]");
                SetData(3, email);
                SetData(4, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[12]/TD[1]/SPAN[0]"));
                String number1 = scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[15]/TD[1]/DIV[0]/DIV[2]/SELECT[2]");
                number1 += "-";
                number1 += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[15]/TD[1]/DIV[0]/DIV[4]/INPUT[0]");
                number1 += "-";
                number1 += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[15]/TD[1]/DIV[0]/DIV[6]/INPUT[0]");
                SetData(5, number1);
                String number2 = scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[16]/TD[1]/DIV[0]/DIV[1]/SELECT[2]");
                number2 += "-";
                number2 += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[16]/TD[1]/DIV[0]/DIV[3]/INPUT[0]");
                number2 += "-";
                number2 += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[16]/TD[1]/DIV[0]/DIV[5]/INPUT[0]");
                SetData(6, number2);
                SetData(7, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/SPAN[19]/DIV[3]/TABLE[0]/TBODY[2]/TR[4]/TD[1]/UL[1]/LI[0]/DIV[1]/TABLE[0]/TBODY[1]/TR[1]/TD[1]/LABEL[0]"));
                SetData(8, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/SPAN[19]/DIV[3]/TABLE[0]/TBODY[2]/TR[4]/TD[1]/UL[1]/LI[0]/DIV[1]/TABLE[0]/TBODY[1]/TR[0]/TD[1]/LABEL[0]"));
                SetData(9, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/SPAN[19]/DIV[3]/TABLE[0]/TBODY[2]/TR[4]/TD[1]/UL[1]/LI[0]/DIV[1]/TABLE[0]/TBODY[1]/TR[2]/TD[1]/LABEL[0]"));
                SetData(10, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[9]/TD[1]/SPAN[0]"));
                SetData(11, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[11]/TD[1]/SPAN[0]"));
                SetData(13, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[11]/TD[3]/SPAN[0]"));
                SetData(14, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[0]/DIV[0]/FORM[24]/DIV[5]/DIV[1]/DIV[17]/TABLE[1]/TBODY[2]/TR[12]/TD[1]/SPAN[0]"));
            }
            catch (Exception e)
            {
                SetEventHandler(ScrapState.ERROR);
                ErrorLog(e.ToString());
            }
        }

        protected override void Finish()
        {
            DebugLog("finish");
            SetEventHandler(ScrapState.COMPLETE);
            // TODO:How do the data insert when login error?
            if (!(bool)GetData(0))
            {
                InfoLog("login error");
                return;
            }

            DebugLog("INSERT");
            DebugLog(ID);
            DebugLog(PW);
            DebugLog(PARAM[0]);
            //It delete data of table in database.
            try
            {
                List<long> IndexList = FactoryDao.GetScrappingDataDao().GetIndex(PARAM[0], "006");
                if (IndexList != null && IndexList.Count > 0)
                {
                    foreach (long index_ in IndexList)
                    {
                        FactoryDao.GetScrappingDataSubDao().Delete(index_.ToString());
                        FactoryDao.GetScrappingDataDao().Delete(index_.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog(e.ToString());
            }
            ScrappingData data = new ScrappingData();
            data.SiteCode = "006";
            data.Id = ID;
            data.ApplyNum = Convert.ToInt64(PARAM[0]);
            data.Data01 = ConvertString(GetData(1));
            data.Data02 = ConvertString(GetData(2));
            data.Data03 = ConvertString(GetData(3));
            data.Data04 = ConvertString(GetData(4));
            data.Data05 = ConvertString(GetData(5));
            data.Data06 = ConvertString(GetData(6));
            data.Data07 = ConvertString(GetData(7));
            data.Data08 = ConvertString(GetData(8));
            data.Data09 = ConvertString(GetData(9));
            data.Data10 = ConvertString(GetData(10));
            data.Data11 = ConvertString(GetData(11));
            data.Data13 = ConvertString(GetData(13));
            data.Data14 = ConvertString(GetData(14));
            int index = FactoryDao.GetScrappingDataDao().Insert(data, true);
            DebugLog("index " + index);

            ScrappingTotalData scraptotal = new ScrappingTotalData();
            FactoryDao.GetScrappingTotalDataDao().Delete(PARAM[0], "006");
            scraptotal.ApplyNum = Convert.ToInt64(PARAM[0]);
            scraptotal.Id = ID;
            scraptotal.SiteCode = "006";
            FactoryDao.GetScrappingTotalDataDao().Insert(scraptotal);


            SetEventHandler(ScrapState.COMPLETE);
        }
        public override string ToString()
        {
            return base.ToString();
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