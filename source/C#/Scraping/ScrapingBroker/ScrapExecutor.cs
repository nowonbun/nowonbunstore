using System;
using System.Diagnostics;
using WebScraping.Library.Log;
using System.Text;

namespace WebScraping.Broker
{
    class ScrapExecutor : Process
    {
        private Logger logger = null;
        public Parameter Parameter { get; private set; }
        public Response ResponseCode { get; private set; }
        
        public ScrapExecutor(Parameter parameter)
        {
            logger = LoggerBuilder.Init().Set(GetType()).Info("ScrapExecutor call");
            this.ResponseCode = new Response(parameter.Key);
            base.StartInfo = new ProcessStartInfo();
            base.StartInfo.FileName = parameter.Exec == ExecType.Only1Scraper ? "WebScraping.Only1Scraper.exe" : "WebScraping.Scraper.exe";
            this.EnableRaisingEvents = true;
            this.Parameter = parameter;
            this.logger.Info(" [NODE LOG] The excuter is generated");
        }
        public ScrapExecutor Run()
        {
            Parameter.Starttime = DateTime.Now;
            String param = this.Parameter.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(param);
            base.StartInfo.Arguments = Convert.ToBase64String(buffer);
            base.Start();
            this.logger.Info(" [NODE LOG] Scraper process Start " + param);
            ControlFactory.GetForm<Main>().SetGrid(this.Parameter);
            return this;
        }
    }
}
