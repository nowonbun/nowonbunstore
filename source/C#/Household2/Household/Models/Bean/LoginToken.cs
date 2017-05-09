using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Bean
{
    public class LoginToken : AbstractBean
    {
        private string access_token;
        private long expires_in;
        private string id_token;
        private string refresh_token;
        private string token_type;

        public string Access_token
        {
            set { access_token = value; }
            get { return access_token; }
        }

        public long Expires_in
        {
            set { expires_in = value; }
            get { return expires_in; }
        }

        public String Id_token
        {
            set { id_token = value; }
            get { return id_token; }
        }

        public String Refresh_token
        {
            set { refresh_token = value; }
            get { return refresh_token; }
        }

        public String Token_type
        {
            set { token_type = value; }
            get { return token_type; }
        }
    }
}