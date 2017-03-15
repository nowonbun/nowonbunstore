using System;
using System.Windows.Forms;
using System.Threading;

namespace ScrapCore
{
    public abstract class AbstractScrappingProcess : IScrappingProcess
    {
        private ScrapState state = ScrapState.INIT;

        private Scrapping scrap = null;
        
        private ScrappingList scrappingList = new ScrappingList();
        
        protected string ID { get; private set; }
        
        protected string PW { get; private set; }
        
        protected string[] PARAM { get; private set; }

        
        public AbstractScrappingProcess(string id, string pw, string[] param)
        {
            this.ID = id;
            this.PW = pw;
            this.PARAM = param;
        }
        
        protected abstract void Initialize();
        
        protected abstract void Execute(IScrapping scrap);
        
        protected abstract void Finish();
        
        protected abstract void Error(Exception e);
        
        protected abstract void Navigated(String url);
        
        protected void SetData(int index, Object data)
        {
            scrappingList[index] = data;
        }
        
        protected Object GetData(int index)
        {
            return scrappingList[index];
        }
        
        public void Close()
        {
            if (scrap != null)
            {
                scrap.Dispose();
            }
        }
        
        public ApplicationContext Run()
        {
            if (this.scrap != null)
            {
                throw new ScrappingException("The instance is already existed");
            }
            this.scrap = new Scrapping((url) => { Navigated(url); });
            ThreadPool.QueueUserWorkItem((c) =>
            {
                try
                {
                    state = ScrapState.RUNNING;
                    Initialize();
                    using (scrap)
                    {
                        try
                        {
                            Execute(scrap);
                        }
                        finally
                        {
                            scrap.Move("about:blank");
                        }
                    }
                    Finish();
                    
                    state = ScrapState.COMPLETE;
                }
                catch (Exception e)
                {
                    state = ScrapState.ERROR;
                    Error(e);
                }
            });
            return scrap.Connection;
        }
        
        public ScrapState GetState()
        {
            return state;
        }
    }
}
