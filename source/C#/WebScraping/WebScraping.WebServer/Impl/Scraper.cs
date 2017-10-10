using System;
using System.Diagnostics;
using System.IO;
using WebScraping.Dao.Common;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Entity;

namespace WebScraping.WebServer.Impl
{
    class Scraper : Process
    {
        private Parameter parameter;
        private String parameterStr;
        public Scraper(String parameter, String path)
        {

            base.StartInfo = new ProcessStartInfo();
            base.StartInfo.FileName = "WebScraping.Scraper.exe";
            this.parameter = SetParamter(parameter);
            this.parameterStr = parameter;
            base.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
        }
        public String Run()
        {
            parameter.Key = System.Guid.NewGuid().ToString();
            base.StartInfo.Arguments = parameter.Key + " " + this.parameterStr;
            
            IScrapingStatusDao dao = FactoryDao.GetInstance().GetDao("WebScraping.Dao.Dao.Impl.ScrapingStatusDao") as IScrapingStatusDao;
            ScrapingStatus entity = new ScrapingStatus();
            entity.KeyCode = parameter.Key;
            entity.SCode = parameter.Code;
            entity.Sid = parameter.Id;
            entity.StartTime = DateTime.Now;
            entity.Status = "0";
            dao.Insert(entity);
            parameter.Starttime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            base.Start();
            return parameter.Key;
        }
        private Parameter SetParamter(String param)
        {
            Parameter ret = new Parameter();
            String[] buffer = param.Split('&');
            foreach(String b in buffer)
            {
                String[] t = b.Split('=');
                if ("CODE".Equals(t[0].ToUpper()))
                {
                    ret.Code = t[1];
                }else if ("ID".Equals(t[0].ToUpper()))
                {
                    ret.Id = t[1];
                }
            }
            return ret;
        }
        public Parameter Parameter
        {
            get { return this.parameter; }
        }
    }
}
