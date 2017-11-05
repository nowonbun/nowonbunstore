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
    /// <summary>
    /// Auther : SoonYub Hwang
    /// Date : 26. 11. 2016
    /// This is System to script Domekuk.
    /// </summary>
    public class Domekuk : AbstractClass, ICommonScrap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Domekuk(string id, string pw, string[] args)
            : base(id, pw, args)
        {

        }
        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {

        }
        /// <summary>
        /// This is thing that scrapping is start.
        /// </summary>
        /// <value>
        /// 1 : 회원정보
        /// 2 : 회원이름
        /// 3 : 이메일
        /// 4 : 주소
        /// 5 : 전화번호
        /// 6 : 핸드폰번호
        /// 7 : 예금주 이름
        /// 8 : 은행 종류
        /// 9 : 계좌 번호
        /// 10 : 회사 이름
        /// 11 : 대표자 이름
        /// 12 : 부서명
        /// 13 : 업종
        /// 14 : 회사 이름
        /// 15 : 회사 전화번호
        /// 16 : 회사 소개
        /// 17 : 사업 종류
        /// 18 : 사업자 번호
        /// 19 : 담당자 이름
        /// 20 : 업태
        /// 21 : 총금액
        /// 22 : 3개월전 입금예정 - 도매꾹
        /// 23 : 2개월전 입금예정 - 도매꾹
        /// 24 : 1개월전 입금예정 - 도매꾹
        /// 25 : 3개월전 승인대기 - 도매꾹
        /// 26 : 2개월전 승인대기 - 도매꾹
        /// 27 : 1개월전 승인대기 - 도매꾹
        /// 28 : 3개월전 발송예정 - 도매꾹
        /// 29 : 2개월전 발송예정 - 도매꾹
        /// 30 : 1개월전 발송예정 - 도매꾹
        /// 31 : 3개월전 배송중 - 도매꾹
        /// 32 : 2개월전 배송중 - 도매꾹
        /// 33 : 1개월전 배송중 - 도매꾹
        /// 34 : 3개월전 적립예정 - 도매꾹
        /// 35 : 2개월전 적립예정 - 도매꾹
        /// 36 : 1개월전 적립예정 - 도매꾹
        /// 37 : 3개월전 승인취소 - 도매꾹
        /// 38 : 2개월전 승인취소 - 도매꾹
        /// 39 : 1개월전 승인취소 - 도매꾹
        /// 40 : 3개월전 판매취소 - 도매꾹
        /// 41 : 2개월전 판매취소 - 도매꾹
        /// 42 : 1개월전 판매취소 - 도매꾹
        /// 43 : 3개월전 구매취소 - 도매꾹
        /// 44 : 2개월전 구매취소 - 도매꾹
        /// 45 : 1개월전 구매취소 - 도매꾹
        /// 46 : 3개월전 입금예정 - 도매매
        /// 47 : 2개월전 입금예정 - 도매매
        /// 48 : 1개월전 입금예정 - 도매매
        /// 49 : 3개월전 승인대기 - 도매매
        /// 50 : 2개월전 승인대기 - 도매매
        /// 51 : 1개월전 승인대기 - 도매매
        /// 52 : 3개월전 발송예정 - 도매매
        /// 53 : 2개월전 발송예정 - 도매매
        /// 54 : 1개월전 발송예정 - 도매매
        /// 55 : 3개월전 배송중 - 도매매
        /// 56 : 2개월전 배송중 - 도매매
        /// 57 : 1개월전 배송중 - 도매매
        /// 58 : 3개월전 적립예정 - 도매매
        /// 59 : 2개월전 적립예정 - 도매매
        /// 60 : 1개월전 적립예정 - 도매매
        /// 61 : 3개월전 승인취소 - 도매매
        /// 62 : 2개월전 승인취소 - 도매매
        /// 63 : 1개월전 승인취소 - 도매매
        /// 64 : 3개월전 판매취소 - 도매매
        /// 65 : 2개월전 판매취소 - 도매매
        /// 66 : 1개월전 판매취소 - 도매매
        /// 67 : 3개월전 구매취소 - 도매매
        /// 68 : 2개월전 구매취소 - 도매매
        /// 69 : 1개월전 구매취소 - 도매매
        /// </value>
        protected override void Execute(IScrapping scrap)
        {
            // TODO : The url of the data of the test is following next:
            // http://localhost:10000/?type=scrap_request&code=002&apply=2&id=storehouse&pw=hana0911!
            try
            {
                DebugLog("START");
                //It deny the tag to speed up scrapping.
                scrap.AddPrintDenyTagName("META");
                scrap.AddPrintDenyTagName("SCRIPT");
                scrap.AddPrintDenyTagName("LINK");
                scrap.AddPrintDenyTagName("NOSCRIPT");
                scrap.AddPrintDenyTagName("STYLE");

                scrap.Move("http://domeggook.com/main/member/mem_formLogin.php?back=L21haW4vaW5kZXgucGhw");
                DebugLog(scrap.GetUrl());
                scrap.SetInputValueByxPath("HTML[0]/BODY[1]/DIV[10]/DIV[0]/FORM[1]/DIV[6]/INPUT[0]", base.ID);
                scrap.SetInputValueByxPath("HTML[0]/BODY[1]/DIV[10]/DIV[0]/FORM[1]/DIV[6]/INPUT[1]", base.PW);
                scrap.ClickByxPath("HTML[0]/BODY[1]/DIV[10]/DIV[0]/FORM[1]/DIV[6]/INPUT[2]");

                DebugLog(scrap.GetUrl());
                String urlBuffer = scrap.GetUrl();
                //it fault login.
                if (urlBuffer.IndexOf("mem_formLogin.php") > 0)
                {
                    SetData(0, false);
                    return;
                }
                SetData(0, true);

                {
                    DebugLog(scrap.GetUrl());
                    //It move site to following next:
                    scrap.Move("http://domeggook.com/main/member/mem_formRegular.php?mode=editRegular&back=L21haW4vaW5kZXgucGhw");

                    LOG.Info("It is scripted  following next: The ID of member - 1");
                    SetData(1, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[0]/TD[1]/B[0]"));

                    LOG.Info("It is scripted  following next: The name of member - 2");
                    SetData(2, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[9]/TD[1]"));

                    LOG.Info("It is scripted  following next: Email - 3");
                    String email = scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[11]/TD[1]/INPUT[0]");
                    email += "@";
                    email += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[11]/TD[1]/INPUT[2]");
                    SetData(3, email);

                    LOG.Info("It is scripted  following next: address - 4");
                    String address = scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[15]/TD[1]/TABLE[2]/TBODY[0]/TR[0]/TD[0]/INPUT[0]");
                    address += " ";
                    address += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[15]/TD[1]/TABLE[2]/TBODY[0]/TR[1]/TD[0]/INPUT[0]");
                    address += " ";
                    address += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[15]/TD[1]/TABLE[2]/TBODY[0]/TR[2]/TD[0]/INPUT[0]");
                    SetData(4, address);

                    LOG.Info("It is scripted  following next: telephone number - 5");
                    String tel = scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[17]/TD[1]/SELECT[0]");
                    tel += " - ";
                    tel += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[17]/TD[1]/INPUT[2]");
                    tel += " - ";
                    tel += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[17]/TD[1]/INPUT[4]");
                    SetData(5, tel);

                    LOG.Info("It is scripted  following next: mobile number. - 6");
                    String hp = scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[19]/TD[1]/SELECT[0]");
                    hp += " - ";
                    hp += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[19]/TD[1]/INPUT[2]");
                    hp += " - ";
                    hp += scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TBODY[3]/TR[19]/TD[1]/INPUT[4]");
                    SetData(6, hp);

                    LOG.Info("It is scripted  following next: The account name of bank - 7");
                    SetData(7, scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[10]/TBODY[2]/TR[0]/TD[1]/INPUT[0]"));

                    LOG.Info("It is scripted  following next: The type of bank - 8");
                    SetData(8, scrap.GetSelectValueByXPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[10]/TBODY[2]/TR[0]/TD[3]/SELECT[0]"));

                    LOG.Info("It is scripted  following next: The account number of bank - 9");
                    SetData(9, scrap.GetInputValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[10]/TBODY[2]/TR[2]/TD[1]/INPUT[0]"));

                    LOG.Info("It is scripted  following next: The name of company - 10");
                    SetData(10, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[0]/TD[1]"));

                    LOG.Info("It is scripted  following next: The name of boss - 11");
                    SetData(11, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[2]/TD[1]"));

                    LOG.Info("It is scripted  following next: The department - 12");
                    SetData(12, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[4]/TD[1]"));

                    LOG.Info("It is scripted  following next: The type of business  13");
                    SetData(13, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[7]/TD[1]"));

                    LOG.Info("It is scripted  following next: The address of company - 14");
                    SetData(14, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[9]/TD[1]"));

                    LOG.Info("It is scripted  following next: The phonenumber of company - 15");
                    SetData(15, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[11]/TD[1]"));

                    LOG.Info("It is scripted  following next: The introduce of company - 16");
                    SetData(16, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[15]/TD[1]"));

                    LOG.Info("It is scripted  following next: The type of business - 17");
                    SetData(17, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[0]/TD[3]"));

                    LOG.Info("It is scripted  following next: Business Number - 18");
                    SetData(18, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[2]/TD[3]"));

                    LOG.Info("It is scripted  following next: Contact person - 19");
                    SetData(19, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[4]/TD[3]"));

                    LOG.Info("It is scripted  following next: Sector - 20");
                    SetData(20, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[13]/FORM[0]/TABLE[17]/TBODY[2]/TR[7]/TD[3]"));
                }
                {
                    DebugLog(scrap.GetUrl());
                    scrap.Move("http://domeggook.com/main/myPage/emoney/my_emoneyList.php?stitle=&quick=&y1=2015&m1=10&d1=01&y2=2016&m2=10&d2=17&x=34&y=13");
                    //The total of price
                    SetData(21, scrap.GetNodeValueByxPath("HTML[0]/BODY[1]/DIV[17]/DIV[1]/DIV[1]/DIV[2]/TABLE[3]/TBODY[1]/TR[1]/TD[7]/B[0]"));
                }
                //TODO: It should partition the data that total of price is scraped.
                String[] site = new String[] { "domeggook.com", "domeme.domeggook.com" };
                String[] modeType = new String[] { "WAITPAY", "WAITCONFIRM", "WAITDELI", "WAITOK", "WAITSERV", "DENYCONFIRM", "DENYSELL", "DENYBUY" };
                int dataindex = 22;
                foreach (String s in site)
                {
                    String divIndex;
                    if ("domeggook.com".Equals(s))
                    {
                        divIndex = "17";
                    }
                    else
                    {
                        divIndex = "32";
                    }
                    foreach (String mode in modeType)
                    {
                        for (int period = 3; period > 0; period--)
                        {
                            DateTime today;
                            DateTime fromDay;
                            GetDate(period, out fromDay, out today);

                            int sum = 0;
                            bool nextpage = true;
                            for (int pageIndex = 0; nextpage; pageIndex++)
                            {
                                scrap.Move("http://" + s + "/main/mySell/sell/my_sellList.php?&mode=" + mode + "&quick=&y1=" + fromDay.Year + "&m1=" + fromDay.Month + "&d1=" + fromDay.Day + "&y2=" + today.Year + "&m2=" + today.Month + "&d2=" + today.Day + "&sno=&stt=&sid=&sqt=&sam=&dtype=o&ob=p&pagenum=" + pageIndex);
                                DebugLog(scrap.GetUrl());
                                nextpage = false;
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
                            LOG.Info("It is scripted  following next: " + dataindex);
                            SetData(dataindex, sum);
                            dataindex++;
                        }
                    }
                }
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
                List<long> IndexList = FactoryDao.GetScrappingDataDao().GetIndex(PARAM[0], "002");
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
            data.SiteCode = "002";
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
            data.Data12 = ConvertString(GetData(12));
            data.Data13 = ConvertString(GetData(13));
            data.Data14 = ConvertString(GetData(14));
            data.Data15 = ConvertString(GetData(15));
            data.Data16 = ConvertString(GetData(16));
            data.Data17 = ConvertString(GetData(17));
            data.Data18 = ConvertString(GetData(18));
            data.Data19 = ConvertString(GetData(19));
            data.Data20 = ConvertString(GetData(20));
            data.Data21 = ConvertString(GetData(21));

            int index = FactoryDao.GetScrappingDataDao().Insert(data, true);
            DebugLog("index " + index);
            Decimal total = 0;
            Decimal expected = 0;
            //TODO: It should partition the data that total of price is scraped.

            for (int i = 0; i < 3; i++)
            {
                ScrappingDataSub sub = new ScrappingDataSub();
                sub.DataIdx = index;
                // 입금예정 - 도매꾹
                sub.Data01 = ConvertString(GetData(22 + i));
                total = Sum(total, ConvertString(GetData(22 + i)));
                LOG.Info("index " + (22 + i) + " : " + ConvertString(GetData(22 + i)));
                // 승인대기 - 도매꾹
                sub.Data02 = ConvertString(GetData(25 + i));
                total = Sum(total, ConvertString(GetData(25 + i)));
                LOG.Info("index " + (25 + i) + " : " + ConvertString(GetData(25 + i)));
                // 발송예정 - 도매꾹
                sub.Data03 = ConvertString(GetData(28 + i));
                total = Sum(total, ConvertString(GetData(28 + i)));
                LOG.Info("index " + (28 + i) + " : " + ConvertString(GetData(28 + i)));
                // 배송중 - 도매꾹
                sub.Data04 = ConvertString(GetData(31 + i));
                total = Sum(total, ConvertString(GetData(31 + i)));
                LOG.Info("index " + (31 + i) + " : " + ConvertString(GetData(31 + i)));
                // 적립예정 - 도매꾹
                sub.Data05 = ConvertString(GetData(34 + i));
                expected = Sum(expected, ConvertString(GetData(34 + i)));
                LOG.Info("index " + (34 + i) + " : " + ConvertString(GetData(34 + i)));
                // 승인취소 - 도매꾹
                sub.Data06 = ConvertString(GetData(37 + i));
                total = Sum(total, ConvertString(GetData(37 + i)));
                LOG.Info("index " + (37 + i) + " : " + ConvertString(GetData(37 + i)));
                // 판매취소 - 도매꾹
                sub.Data07 = ConvertString(GetData(40 + i));
                total = Sum(total, ConvertString(GetData(40 + i)));
                LOG.Info("index " + (40 + i) + " : " + ConvertString(GetData(40 + i)));
                // 구매취소 - 도매꾹
                sub.Data08 = ConvertString(GetData(43 + i));
                total = Sum(total, ConvertString(GetData(43 + i)));
                LOG.Info("index " + (43 + i) + " : " + ConvertString(GetData(43 + i)));
                // 입금예정 - 도매매
                sub.Data09 = ConvertString(GetData(46 + i));
                total = Sum(total, ConvertString(GetData(46 + i)));
                LOG.Info("index " + (46 + i) + " : " + ConvertString(GetData(46 + i)));
                // 승인대기 - 도매매
                sub.Data10 = ConvertString(GetData(49 + i));
                total = Sum(total, ConvertString(GetData(49 + i)));
                LOG.Info("index " + (49 + i) + " : " + ConvertString(GetData(49 + i)));
                // 발송예정 - 도매매
                sub.Data11 = ConvertString(GetData(52 + i));
                total = Sum(total, ConvertString(GetData(52 + i)));
                LOG.Info("index " + (52 + i) + " : " + ConvertString(GetData(52 + i)));
                // 배송중 - 도매매
                sub.Data12 = ConvertString(GetData(55 + i));
                total = Sum(total, ConvertString(GetData(55 + i)));
                LOG.Info("index " + (55 + i) + " : " + ConvertString(GetData(55 + i)));
                // 적립예정 - 도매매
                sub.Data13 = ConvertString(GetData(58 + i));
                total = Sum(total, ConvertString(GetData(58 + i)));
                LOG.Info("index " + (58 + i) + " : " + ConvertString(GetData(58 + i)));
                // 승인취소 - 도매매
                sub.Data14 = ConvertString(GetData(61 + i));
                total = Sum(total, ConvertString(GetData(61 + i)));
                LOG.Info("index " + (61 + i) + " : " + ConvertString(GetData(61 + i)));
                // 판매취소 - 도매매
                sub.Data15 = ConvertString(GetData(64 + i));
                total = Sum(total, ConvertString(GetData(64 + i)));
                LOG.Info("index " + (64 + i) + " : " + ConvertString(GetData(64 + i)));
                // 구매취소 - 도매매
                sub.Data16 = ConvertString(GetData(67 + i));
                total = Sum(total, ConvertString(GetData(67 + i)));
                LOG.Info("index " + (67 + i) + " : " + ConvertString(GetData(67 + i)));

                FactoryDao.GetScrappingDataSubDao().Insert(sub);
            }

            ScrappingTotalData scraptotal = new ScrappingTotalData();
            FactoryDao.GetScrappingTotalDataDao().Delete(PARAM[0], "002");
            scraptotal.ApplyNum = Convert.ToInt64(PARAM[0]);
            scraptotal.Id = ID;
            scraptotal.SiteCode = "002";
            scraptotal.SettlementTotalFee = total.ToString();
            scraptotal.ExpectFee = expected.ToString();
            FactoryDao.GetScrappingTotalDataDao().Insert(scraptotal);
        }
        private Decimal Sum(Decimal sum, String data)
        {
            try
            {
                return decimal.Add(sum, decimal.Parse(data));
            }
            catch
            {
                return sum;
            }
        }
        public override string ToString()
        {
            return base.ToString();
            /*StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 37; i++)
            {
                object buffer = GetData(i);
                String data = buffer == null ? "" : buffer.ToString();
                sb.Append("Index:" + i + " Length:" + data.Length);
                sb.AppendLine();
                sb.AppendLine(data);
            }
            return sb.ToString();*/
        }
        protected override void Navigated(string url)
        {
            DebugLog("Navigated : " + url);
            //TODO : If the page what demand to change password appear,that process it nothing.
            //Hence, you should make it.

        }
        protected override void Error(Exception e)
        {
            ErrorLog(e);
            SetEventHandler(ScrapState.ERROR);
        }
    }
}