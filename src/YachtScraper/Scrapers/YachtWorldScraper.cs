using Microsoft.Playwright;
using System;
using System.Linq;
using YachtScraper.Models;
using YachtScraper.Services;

namespace YachtScraper.Scrapers;
public partial class YachtWorldScraper:ScraperBase{
    public static class Configs {
        public static string BaseUrl { get;} = "https://www.yachtworld.com/";
        public static string SearchLocator ="button[aria-label='Search']";
    }
    
    ILocator _searchButton;

    public YachtWorldScraper(IPage page, ScrapingMode scrapingMode=ScrapingMode.PageContent):base(page,Configs.BaseUrl, scrapingMode) {
        _searchButton=Page.Locator(Configs.SearchLocator);
    }
    public override async Task<List<YachtListing>> DoScrapingAsync()
    {
        await ScrapingService.RandomWait();
        await _searchButton.ClickAsync();
        await Task.Delay(5000);
        var result = new List<YachtListing>();
        result.Add(new()
        {
            Title = "Luxury Yacht",
            Price = 1_200_200,
            Location = "Miami, FL",
            Url = "https://example.com/yacht"
        });
        return result;
    }

}
