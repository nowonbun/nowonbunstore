using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Bean
{
    public class LoginBean : AbstractBean
    {
        private string inputGroup;
        private string inuptId;
        private string inputPassword;
        private string errorMessage;
        private string post;
        private string remember;

        public string InputGroup
        {
            get { return inputGroup; }
            set { inputGroup = value; }
        }

        public string InuptId
        {
            get { return inuptId; }
            set { inuptId = value; }
        }

        public string InputPassword
        {
            get { return inputPassword; }
            set { inputPassword = value; }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
        public string Remember
        {
            get { return remember; }
            set { remember = value; }
        }

        public string Post
        {
            get { return post; }
            set { post = value; }
        }
    }
}