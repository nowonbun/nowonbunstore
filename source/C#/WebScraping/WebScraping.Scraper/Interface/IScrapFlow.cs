using System;
using Gecko;
using Gecko.DOM;

namespace WebScraping.Scraper.Interface
{
    public interface IScrapFlow
    {
        String StartPage();
        Func<GeckoDocument, Uri, Boolean> Procedure(Uri uri);
        Action<String, String> DownloadProcedure(String url);
        void End();
    }
}
