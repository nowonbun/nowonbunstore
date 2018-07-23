using System;
using Newtonsoft.Json;

namespace WebScraping
{
    public class Response
    {
        public String Key { get; private set; }
        public ResultCode Resultcode { get; set; }
        public String ResultCD
        {
            get
            {
                return Resultcode.ToString();
            }
        }
        public ResultType Resulttype { get; set; }
        public String Starttime { get; set; }
        public String Endtime { get; set; }
        public String ResultMSG { get; set; }

        public Response(String key)
        {
            this.Key = key;
            this.Resultcode = ResultCode.RC9001;
        }
        public void SetResultCode(ResultCode code)
        {
            Resultcode = code;
            switch (Resultcode)
            {
                case ResultCode.RC1000:
                    ResultMSG = "정상";
                    return;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string ToJson()
        {
            return ToString();
        }
        public static Response Create(string json)
        {
            return JsonConvert.DeserializeObject<Response>(json);
        }
    }
}
