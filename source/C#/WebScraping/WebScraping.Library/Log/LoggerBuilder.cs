using System;
using System.IO;
using WebScraping.Library.Exception;
using log4net;
using log4net.Config;

namespace WebScraping.Library.Log
{
    public sealed class LoggerBuilder
    {
        private LoggerBuilder()
        {

        }
        private static String logPath = null;

        public static Logger Init()
        {
            if (LoggerBuilder.logPath != null)
            {
                FileInfo file = new FileInfo(logPath);
                if (!file.Exists)
                {
                    throw new LogException("Not found file of log setting");
                }
                XmlConfigurator.Configure(file);
            }
            else
            {
                XmlConfigurator.Configure();
            }
            return new Logger();
        }
        public static Logger Init(String logPath)
        {
            if (LoggerBuilder.logPath == null)
            {
                LoggerBuilder.logPath = logPath;
            }
            return Init();
        }
    }
}
