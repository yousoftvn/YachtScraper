using Microsoft.Playwright;
using System;
using System.Linq;

namespace YachtScraper.Scrapers;
public class YachtWorldScraper:ScraperBase{

    public static class Configs {
        public static string BaseUrl { get;} = "https://www.yachtworld.com/";
    }

    public YachtWorldScraper(IPage page, ScrapingMode scrapingMode=ScrapingMode.PageContent):base(page,Configs.BaseUrl, scrapingMode) {

    }

}
