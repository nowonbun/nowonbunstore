using System;
using System.IO;
using WebScraping.Library.Exception;
using log4net;
using log4net.Config;

namespace WebScraping.Library.Log
{
    public sealed class Logger
    {
        private ILog logger = null;
        private static String logPath = null;

        public static Logger Init()
        {
            if (Logger.logPath != null)
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
        public Logger Init(String logPath)
        {
            if (logPath == null)
            {
                Logger.logPath = logPath;
            }
            return Init();
        }
        private Logger()
        {

        }
        public void Set(String name)
        {
            logger = LogManager.GetLogger(name);
        }
        public void Set(Type type)
        {
            logger = LogManager.GetLogger(type);
        }
        public void Info(String message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Info(message);
        }
        public void Debug(String message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Debug(message);
        }
        public void Warn(String message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Warn(message);
        }
        public void Error(String message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Error(message);
        }
    }
}
