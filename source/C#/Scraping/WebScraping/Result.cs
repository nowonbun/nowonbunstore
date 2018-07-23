using System;
using Newtonsoft.Json;

namespace WebScraping
{
    public enum ResultType
    {
        Common,
        Pacakage,
        Exit,
        Error
    }

    public class Result
    {
        public String Key { get; private set; }
        public ResultType Resulttype { get; set; }
        public int Index { get; set; }
        public int Separation { get; set; }
        public String Data { get; set; }

        public Result(String key)
        {
            this.Key = key;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToJson()
        {
            return ToString();
        }

        public static Result Build(string json)
        {
            return JsonConvert.DeserializeObject<Result>(json);
        }
    }
}
