using System;
using WebScraping.Library.Log;

namespace WebScraping.Scraper.Other
{
    public class ScraperException : Exception
    {
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScraperException));
        public ScraperException(string message)
            : base(message)
        {
            logger.Error("ScraperExeption Message - " + message);
        }
    }
}
