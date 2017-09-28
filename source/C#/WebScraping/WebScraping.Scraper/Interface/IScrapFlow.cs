using System;
using Gecko;
using Gecko.DOM;

namespace WebScraping.Scraper.Interface
{
    public interface IScrapFlow
    {
        String StartPage();
        Action<GeckoDocument, Uri> Procedure(Uri uri);
    }
}
