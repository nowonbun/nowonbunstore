using System;
using ScrappingCore;
using log4net;
using log4net.Config;

namespace ScrappingServer
{
    public abstract class AbstractClass : AbstractScrappingProcess
    {
        private ILog logger = null;
        private Action<ScrapState> eventhandler = null;
        public AbstractClass(string id, string pw, string[] args)
            : base(id, pw, args)
        {
            logger = LogManager.GetLogger(GetType());
        }
        public ILog LOG
        {
            get { return logger; }
        }
        public void DebugLog(object message)
        {
            if (message != null)
            {
                //ScrappingForm.SetFormLog("DEBUG - " + message.ToString());
                LOG.Debug(message);
            }
        }
        public void InfoLog(object message)
        {
            if (message != null)
            {
                ScrappingForm.SetFormLog("Info - " + message.ToString());
                LOG.Info(message);
            }
        }
        public void ErrorLog(object message)
        {
            if (message != null)
            {
                ScrappingForm.SetFormLog("Error - " + message.ToString());
                LOG.Error(message);
            }
        }
        public void SetHandler(Action<ScrapState> eventhandler)
        {
            this.eventhandler = eventhandler;
        }
        protected void SetEventHandler(ScrapState state)
        {
            if (eventhandler != null)
            {
                eventhandler(state);
            }
        }
        public String ConvertString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return obj.ToString();
        }
        public void GetDate(int period, out DateTime fromDate, out DateTime toDate)
        {
            DateTime now = DateTime.Now;
            fromDate = now.AddMonths(period * -1).AddDays(now.Day * -1).AddDays(1);
            toDate = now.AddMonths((period - 1) * -1).AddDays(now.Day * -1);
        }
    }
}
