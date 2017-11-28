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
        private static Stream logStream = null;

        public static Logger Init()
        {
            /*try
            {
                if (LoggerBuilder.logStream != null)
                {
                    XmlConfigurator.Configure(logStream);
                }
                else if (LoggerBuilder.logPath != null)
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
            }
            catch { }*/
            //XmlConfigurator.Configure();
            return new Logger();
        }
        public static Logger Init(String logPath)
        {
            if (LoggerBuilder.logPath == null && LoggerBuilder.logStream == null)
            {
                LoggerBuilder.logPath = logPath;
                FileInfo file = new FileInfo(logPath);
                if (!file.Exists)
                {
                    throw new LogException("Not found file of log setting");
                }
                XmlConfigurator.Configure(file);
            }
            return Init();
        }
        public static Logger Init(Stream stream)
        {
            if (LoggerBuilder.logPath == null && LoggerBuilder.logStream == null)
            {
                LoggerBuilder.logStream = stream;
                XmlConfigurator.Configure(logStream);
            }
            return Init();
        }
    }
}
