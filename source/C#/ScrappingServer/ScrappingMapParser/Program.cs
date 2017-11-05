using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrappingCore;
using mshtml;

namespace ScrappingMapParser
{
    public class Domekuk : AbstractScrappingProcess
    {
        public Domekuk(string id, string pw, string[] args)
            : base(id, pw, args)
        {

        }
        protected override void Initialize()
        {

        }

        protected override void Execute(IScrapping scrap)
        {
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

            scrap.PrintElementXPath("d:\\mappath1.csv");
            
            Console.WriteLine(scrap.GetUrl());
            Console.WriteLine("Complete");
        }

        protected override void Finish()
        {
          
        }

        protected override void Navigated(string url)
        {
            LOG.Debug("Navigated : " + url);
        }
        protected override void Error(Exception e)
        {

        }
        class Program
        {
            static void Main(string[] args)
            {
                ScriptHook hook = new ScriptHook();
                String scrapcode = AdapterScrapping.Instance().RunScrapping((scrapcode1, code, id, pw, arg) =>
                {
                    Console.WriteLine("adap  " + code);
                    return new Domekuk(id, pw, arg);
                //}, "002", "storehouse", "hana0911!","test");
                }, "001", "wogjsl0213", "dhwogjs02!","test");
                Console.WriteLine(scrapcode);
                Console.WriteLine("Press Any Key...");
                Console.ReadLine();
                hook.Dispose();
            }
        }
    }
}
