using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Models.Bean
{
    public class AjaxResultBean : Dictionary<String,object>
    {
        public String Result
        {
            set
            {
                if (ContainsKey("result"))
                {
                    Remove("result");
                }
                Add("result", value);
            }
            get
            {
                if (!ContainsKey("result"))
                {
                    Add("result", "");
                }
                return (String)this["result"];
            }
        }
        public String Error
        {
            set
            {
                if (ContainsKey("error"))
                {
                    Remove("error");
                }
                Add("error", value);
            }
            get
            {
                if (!ContainsKey("error"))
                {
                    Add("error", "");
                }
                return (String)this["error"];
            }
        }
    }
}