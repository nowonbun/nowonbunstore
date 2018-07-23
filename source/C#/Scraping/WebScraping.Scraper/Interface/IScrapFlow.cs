using System;
using Gecko;
using WebScraping.Scraper.Common;

namespace WebScraping.Scraper.Interface
{
    public interface IScrapFlow
    {
        void Initialize(FlowModelData flowModelData);
        String StartPage(FlowModelData flowModelData);
        void Procedure(Uri uri, out Action<FlowModelData> flow, out String next, out Action<FlowModelData> callback);
        void End(FlowModelData flowModelData);
        void Error(Exception e);
    }
}
