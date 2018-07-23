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
        public Logger Set(String name)
        {
            logger = LogManager.GetLogger(name);
            return this;
        }
        public Logger Set(Type type)
        {
            logger = LogManager.GetLogger(type);
            return this;
        }
        public Logger Info(Object message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Info(message);
            return this;
        }
        public Logger Debug(Object message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Debug(message);
            return this;
        }
        public Logger Warn(Object message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Warn(message);
            return this;
        }
        public Logger Error(Object message)
        {
            if (logger == null)
            {
                throw new LogException("Not initialize that is Set");
            }
            logger.Error(message);
            return this;
        }
    }
}
