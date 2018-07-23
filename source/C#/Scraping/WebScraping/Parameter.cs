using System;
using Newtonsoft.Json;

namespace WebScraping
{
    public class Parameter
    {
        public String Key { get; set; }
        public int MallCD { get; set; }
        public String Id1 { get; set; }
        public String Id2 { get; set; }
        public String Id3 { get; set; }
        public String Pw1 { get; set; }
        public String Pw2 { get; set; }
        public String Pw3 { get; set; }
        public String Option1 { get; set; }
        public String Option2 { get; set; }
        public String Option3 { get; set; }
        public String Sdate { get; set; } // 매출내역, 정산내역, 반품율 스크랩 기간 yyyyMM 데이터
        public String Edate { get; set; } // 매출내역, 정산내역, 반품율 스크랩 기간 yyyyMM 데이터
        public ExecType Exec { get; set; } // 실행할 Scraper : 0 -> Only1Scraper, 1 -> Scraper
        public ScrapType ScrapType { get; set; } // 요청타입
        public DateTime Starttime { get; set; } // Scraper 실행시간
        public DateTime Pingtime { get; set; } // Scraper 중간 체크 시간
        public String State { get; set; } // Scraper 상태 표시 -> Only1Scraper는 ScrapType와 동일하게 움직여서 99번 요청이 들어 오는 경우가 아니면 필요 없을듯 합니다. Only1Scraper는 99번을 한번에 처리 할수 있도록 함. Exec가 Only1Scraper이면 manager가 각 Scraptype으로 나누지 않아도 됨(추후 적용 예정) : Only1Scraper는 월별 합계금액으로 데이터 생성하므로 가능함

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToJson()
        {
            return ToString();
        }

        public static Parameter Build(string json)
        {
            return JsonConvert.DeserializeObject<Parameter>(json);
        }
    }
}
