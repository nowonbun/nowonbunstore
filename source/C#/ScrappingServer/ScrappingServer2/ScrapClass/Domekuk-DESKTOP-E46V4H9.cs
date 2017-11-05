using System;
using System.Text;
using System.IO;
using ScrappingCore;
using log4net;
using log4net.Config;

namespace ScrappingServer2
{
    public class Domekuk : AbstractClass
    {
        private Action<ScrapState> eventhandler = null;

        public Domekuk(string id, string pw, string[] args)
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
                LOG.Debug("START");
                scrap.AddPrintDenyTagName("META");
                scrap.AddPrintDenyTagName("SCRIPT");
                scrap.AddPrintDenyTagName("LINK");
                scrap.AddPrintDenyTagName("NOSCRIPT");
                scrap.AddPrintDenyTagName("STYLE");
                scrap.Move("http://domeggook.com/main/member/mem_formLogin.php?back=L21haW4vaW5kZXgucGhw");
                LOG.Debug(scrap.GetUrl());
                scrap.SetInputValueByxPath("HTML[0]/BODY[1]/DIV[10]/DIV[0]/FORM[1]/DIV[6]/INPUT[0]", base.ID);
                scrap.SetInputValueByxPath("HTML[0]/BODY[1]/DIV[10]/DIV[0]/FORM[1]/DIV[6]/INPUT[1]", base.PW);
                scrap.ClickByxPath("HTML[0]/BODY[1]/DIV[10]/DIV[0]/FORM[1]/DIV[6]/INPUT[2]");
                LOG.Debug(scrap.GetUrl());

                {
                    LOG.Debug(scrap.GetUrl());
                    scrap.Move("http://domeggook.com/main/member/mem_formRegular.php?mode=editRegular&back=L21haW4vaW5kZXgucGhw");
                    //회원아이디
                    SetData(1, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[0]/TD[1]/B[0]"));
                    //회원이름
                    SetData(2, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[9]/TD[1]"));
                    //이메일주소
                    String email = scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[11]/TD[1]/INPUT[0]");
                    email += "@";
                    email += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[11]/TD[1]/INPUT[2]");
                    SetData(3, email);
                    //주소
                    String address = scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[15]/TD[1]/TABLE[2]/TBODY[0]/TR[0]/TD[0]/INPUT[0]");
                    address += " ";
                    address += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[15]/TD[1]/TABLE[2]/TBODY[0]/TR[1]/TD[0]/INPUT[0]");
                    address += " ";
                    address += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[15]/TD[1]/TABLE[2]/TBODY[0]/TR[2]/TD[0]/INPUT[0]");
                    SetData(4, address);
                    //전화번호
                    String tel = scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[17]/TD[1]/SELECT[0]");
                    tel += " - ";
                    tel += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[17]/TD[1]/INPUT[2]");
                    tel += " - ";
                    tel += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[17]/TD[1]/INPUT[4]");
                    SetData(5, tel);
                    //휴대전화번호
                    String hp = scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[19]/TD[1]/SELECT[0]");
                    hp += " - ";
                    hp += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[19]/TD[1]/INPUT[2]");
                    hp += " - ";
                    hp += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[19]/TD[1]/INPUT[4]");
                    SetData(6, hp);
                    //예금주명
                    SetData(7, scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[10]/TBODY[2]/TR[0]/TD[1]/INPUT[0]"));
                    //은행선택
                    SetData(8, scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[10]/TBODY[2]/TR[0]/TD[3]/SELECT[0]"));
                    //계좌번호
                    SetData(9, scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[10]/TBODY[2]/TR[2]/TD[1]/INPUT[0]"));
                    //상호명
                    SetData(10, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[0]/TD[1]"));
                    //대표자명
                    SetData(11, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[2]/TD[1]"));
                    //부서명
                    SetData(12, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[4]/TD[1]"));
                    //업태
                    SetData(13, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[7]/TD[1]"));
                    //사업자주소
                    SetData(14, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[9]/TD[1]"));
                    //사업자대표번호
                    SetData(15, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[11]/TD[1]"));
                    //사업자소개
                    SetData(16, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[15]/TD[1]"));
                    //사업자유형
                    SetData(17, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[0]/TD[3]"));
                    //사업자등록번호
                    SetData(18, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[2]/TD[3]"));
                    //담당자명
                    SetData(19, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[4]/TD[3]"));
                    //업종
                    SetData(20, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[7]/TD[3]"));
                }
                {
                    LOG.Debug(scrap.GetUrl());
                    scrap.Move("http://domeggook.com/main/myPage/emoney/my_emoneyList.php?stitle=&quick=&y1=2015&m1=10&d1=01&y2=2016&m2=10&d2=17&x=34&y=13");
                    //총잔액
                    SetData(21, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[3]/TBODY[1]/TR[1]/TD[7]/B[0]"));
                }
                String[] site = new String[] { "domeggook.com", "domeme.domeggook.com" };
                String[] modeType = new String[] { "WAITPAY", "WAITCONFIRM", "WAITDELI", "WAITOK", "WAITSERV", "DENYCONFIRM", "DENYCONFIRM", "DENYBUY" };
                int dataindex = 22;
                foreach (String s in site)
                {
                    String divIndex;
                    if ("domeggook.com".Equals(site))
                    {
                        divIndex = "17";
                    }
                    else
                    {
                        divIndex = "32";
                    }
                    foreach (String mode in modeType)
                    {
                        DateTime today = DateTime.Now;
                        DateTime fromDay = today.AddMonths(-3);
                        int sum = 0;
                        bool nextpage = true;
                        for (int pageIndex = 0; nextpage; pageIndex++)
                        {
                            scrap.Move("http://" + s + "/main/mySell/sell/my_sellList.php?&mode=" + mode + "&quick=&y1=" + fromDay.Year + "&m1=" + fromDay.Month + "&d1=" + fromDay.Day + "&y2=" + today.Year + "&m2=" + today.Month + "&d2=" + today.Day + "&sno=&stt=&sid=&sqt=&sam=&dtype=o&ob=p&pagenum=" + pageIndex);
                            LOG.Debug(scrap.GetUrl());
                            nextpage = false;
                            //scrap.PrintElementXPath("d:\\mapPath.csv");
                            for (int line = 2; true; line++)
                            {
                                if (!scrap.IsElementByXPath("HTML[0]/BODY[1]/DIV[" + divIndex + "]/DIV[1]/DIV[1]/DIV[2]/FORM[1]/TABLE[4]/TBODY[0]/TR[" + line + "]/TD[5]/DIV[0]"))
                                {
                                    break;
                                }
                                nextpage = true;
                                String value = scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[" + divIndex + "]/DIV[1]/DIV[1]/DIV[2]/FORM[1]/TABLE[4]/TBODY[0]/TR[" + line + "]/TD[5]/DIV[0]");
                                value = value.Replace(",", "").Replace("원", "");
                                sum += Convert.ToInt32(value);
                            }
                        }
                        SetData(dataindex, sum);
                        dataindex++;
                    }
                }
            }
            catch (Exception e)
            {
                if (this.eventhandler != null)
                {
                    eventhandler(ScrapState.ERROR);
                }
                LOG.Error(e.ToString());
            }
        }

        public void SetHandler(Action<ScrapState> eventhandler)
        {
            this.eventhandler = eventhandler;
        }

        protected override void Finish()
        {
            LOG.Debug("finish");
            if (this.eventhandler != null)
            {
                eventhandler(ScrapState.COMPLETE);
            }
            FileInfo file = new FileInfo("d:\\result" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt");
            if (file.Exists)
            {
                file.Delete();
            }
            WriteData(file, "회원아이디" + " : " + GetData(1));
            WriteData(file, "회원이름" + " : " + GetData(2));
            WriteData(file, "이메일주소" + " : " + GetData(3));
            WriteData(file, "주소" + " : " + GetData(4));
            WriteData(file, "전화번호" + " : " + GetData(5));
            WriteData(file, "휴대전화번호" + " : " + GetData(6));
            WriteData(file, "예금주명" + " : " + GetData(7));
            WriteData(file, "은행선택" + " : " + GetData(8));
            WriteData(file, "계좌번호" + " : " + GetData(9));
            WriteData(file, "상호명" + " : " + GetData(10));
            WriteData(file, "대표자명" + " : " + GetData(11));
            WriteData(file, "부서명" + " : " + GetData(12));
            WriteData(file, "업태" + " : " + GetData(13));
            WriteData(file, "사업자주소" + " : " + GetData(14));
            WriteData(file, "사업자대표번호" + " : " + GetData(15));
            WriteData(file, "사업자소개" + " : " + GetData(16));
            WriteData(file, "사업자유형" + " : " + GetData(17));
            WriteData(file, "사업자등록번호" + " : " + GetData(18));
            WriteData(file, "담당자명" + " : " + GetData(19));
            WriteData(file, "업종" + " : " + GetData(20));
            WriteData(file, "총잔액" + " : " + GetData(21));
            WriteData(file, "도매꾹(입금예정)" + " : " + GetData(22));
            WriteData(file, "도매꾹(승인대기)" + " : " + GetData(23));
            WriteData(file, "도매꾹(발송예정)" + " : " + GetData(24));
            WriteData(file, "도매꾹(배송중)" + " : " + GetData(25));
            WriteData(file, "도매꾹(적립예정)" + " : " + GetData(26));
            WriteData(file, "도매꾹(승인취소)" + " : " + GetData(27));
            WriteData(file, "도매꾹(판매취소)" + " : " + GetData(28));
            WriteData(file, "도매꾹(구매취소)" + " : " + GetData(29));
            WriteData(file, "도매매(입금예정)" + " : " + GetData(30));
            WriteData(file, "도매매(승인대기)" + " : " + GetData(31));
            WriteData(file, "도매매(발송예정)" + " : " + GetData(32));
            WriteData(file, "도매매(배송중)" + " : " + GetData(33));
            WriteData(file, "도매매(적립예정)" + " : " + GetData(34));
            WriteData(file, "도매매(승인취소)" + " : " + GetData(35));
            WriteData(file, "도매매(판매취소)" + " : " + GetData(36));
            WriteData(file, "도매매(구매취소)" + " : " + GetData(37));
        }
        private void WriteData(FileInfo file, String Data)
        {
            using (FileStream stream = new FileStream(file.FullName, FileMode.Append, FileAccess.Write))
            {
                byte[] data = Encoding.UTF8.GetBytes(Data + "\r\n");
                stream.Write(data, 0, data.Length);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 37; i++)
            {
                object buffer = GetData(i);
                String data = buffer == null ? "" : buffer.ToString();
                sb.Append("Index:" + i + " Length:" + data.Length);
                sb.AppendLine();
                sb.AppendLine(data);
            }
            return sb.ToString();
        }
        protected override void Navigated(string url)
        {
            LOG.Debug("Navigated : " + url);
        }
        protected override void Error(Exception e)
        {
            LOG.Error(e);
            eventhandler(ScrapState.ERROR);
        }
    }
}