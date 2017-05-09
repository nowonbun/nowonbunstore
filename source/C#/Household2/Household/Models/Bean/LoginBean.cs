using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Bean
{
    public class LoginBean : AbstractBean
    {
        private string id;
        private string name;
        private string given_name;
        private string family_name;
        private string link;
        private string picture;
        private string gender;
        private string locale;
        private LoginToken token;

        public string Id
        {
            set { id = value; }
            get { return id; }
        }

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public string Given_name
        {
            set { given_name = value; }
            get { return given_name; }
        }

        public string Family_name
        {
            set { family_name = value; }
            get { return family_name; }
        }

        public string Link
        {
            set { link = value; }
            get { return link; }
        }

        public string Picture
        {
            set { picture = value; }
            get { return picture; }
        }

        public string Gender
        {
            set { gender = value; }
            get { return gender; }
        }

        public string Locale
        {
            set { locale = value; }
            get { return locale; }
        }

        public LoginToken Token
        {
            set { token = value; }
            get { return token; }
        }
    }
}