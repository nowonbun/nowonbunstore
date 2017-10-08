using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Node;
using Gecko;
using Gecko.DOM;
using WebScraping.Library.Log;
using WebScraping.Scraper.Impl;
using WebScraping.Dao.Entity;
using WebScraping.Dao.Common;
using WebScraping.Dao.Dao;
using WebScraping.Dao.Attribute;

namespace WebScraping.Scraper.Common
{
    abstract class AbstractScrapFlow : Allocation, IScrapFlow
    {
        protected ScrapParameter Parameter { get; private set; }
        protected String StartPageUrl { get; set; }
        protected Dictionary<String, Func<GeckoDocument, Uri, Boolean>> FlowMap = new Dictionary<string, Func<GeckoDocument, Uri, Boolean>>();
        private ScrapBrowser browser;
        private IList<ScrapingCommonData> commondata = new List<ScrapingCommonData>();
        private IList<ScrapingPackageData> packagedata = new List<ScrapingPackageData>();

        [ResourceDao]
        private IScrapingCommonDataDao commondao;
        [ResourceDao]
        private IScrapingPakageDataDao packagedao;

        protected Logger logger { get; private set; }
        public AbstractScrapFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
        {
            Parameter = param;
            this.browser = browser;
            this.commondao = FactoryDao.GetInstance().GetDao("WebScraping.Dao.Dao.Impl.ScrapingCommonDataDao") as IScrapingCommonDataDao;
            this.packagedao = FactoryDao.GetInstance().GetDao("WebScraping.Dao.Dao.Impl.ScrapingPakageDataDao") as IScrapingPakageDataDao;
            logger = LoggerBuilder.Init().Set(this.GetType());
        }
        protected virtual Boolean NotAction(GeckoDocument document, Uri uri)
        {
            logger.Info("NotAction uri : " + uri);
            return true;
        }
        public String StartPage()
        {
            return StartPageUrl;
        }
        public Func<GeckoDocument, Uri, Boolean> Procedure(Uri uri)
        {
            logger.Info("Procedure uri : " + uri);
            String key = FlowMap.Keys.Where(k => { return uri.ToString().IndexOf(k) != -1; }).SingleOrDefault();
            if (key == null)
            {
                return NotAction;
            }
            return FlowMap[key];
        }
        protected void Navigate(String url)
        {
            this.browser.Navigate(url);
        }
        protected void SetCommonData(int index, String data)
        {
            ScrapingCommonData node = new ScrapingCommonData();
            node.KeyCode = this.Parameter.Keycode;
            node.keyIndex = index;
            node.Data = data;
            node.CreateDate = DateTime.Now;
            commondata.Add(node);
        }
        protected void SetPackageData(int index, int separation, String data)
        {
            ScrapingPackageData node = new ScrapingPackageData();
            node.KeyCode = this.Parameter.Keycode;
            node.keyIndex = index;
            node.Separation = separation;
            node.Data = data;
            node.CreateDate = DateTime.Now;
            packagedata.Add(node);
        }
        protected void UpdateData()
        {
            foreach(ScrapingCommonData node in commondata)
            {
                commondao.Insert(node);
            }
            foreach(ScrapingPackageData node in packagedata)
            {
                packagedao.Insert(node);
            }
        }
        public void End()
        {
            UpdateData();
            Finally();
        }
        protected abstract void Finally();
    }
}
