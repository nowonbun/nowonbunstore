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
    public class Timon : AbstractClass,ICommonScrap
    {
        public Timon(string id, string pw, string[] args)
            : base(id, pw, args)
        {

        }
        protected override void Initialize()
        {

        }

        protected override void Execute(IScrapping scrap)
        {

        }

        public String ConvertString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return obj.ToString();
        }

        protected override void Finish()
        {
            SetEventHandler(ScrapState.COMPLETE);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
        protected override void Navigated(string url)
        {

        }
        protected override void Error(Exception e)
        {
            ErrorLog(e);
            SetEventHandler(ScrapState.ERROR);
        }
    }
}