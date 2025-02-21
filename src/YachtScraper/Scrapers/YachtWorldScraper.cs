using Microsoft.Playwright;
using System;
using System.Linq;
using YachtScraper.Models;
using YachtScraper.Services;

namespace YachtScraper.Scrapers;
public partial class YachtWorldScraper:ScraperBase{
    public static class Configs {
        public static readonly string BaseUrl = "https://www.yachtworld.com/";
        public static class Locators{
            public static readonly string SearchButton ="#button-search";
            public static readonly string YachtElement="div.container.sponsored.listing-block-1 a.grid-listing-link";
            public static readonly string NameElement="h2[data-e2e='listingName']";
            public static readonly string PriceElement="p[data-e2e='listingPrice']";
            
            public static readonly string LocationElement="p[data-e2e='listingSellerContent']";
        }
    }

    public YachtWorldScraper(IPage page, ScrapingMode scrapingMode=ScrapingMode.PageContent):base(page,Configs.BaseUrl, scrapingMode) {
        
    }
    public override async Task<List<YachtListing>> DoScrapingAsync()
    {
        await Page.WaitForSelectorAsync(Configs.Locators.SearchButton);
        await Page.Locator(Configs.Locators.SearchButton).ClickAsync();
        await Page.WaitForSelectorAsync(Configs.Locators.YachtElement);
        var yachts = new List<YachtListing>();
        var yachtElements = await Page.QuerySelectorAllAsync(Configs.Locators.YachtElement);
          foreach (var yachtElement in yachtElements)
        {
            if(yachtElement is null) continue;
            var nameElement =await yachtElement.QuerySelectorAsync(Configs.Locators.NameElement);
            if(nameElement is null) continue;
            string name = await nameElement.InnerTextAsync();
            var priceElement = await yachtElement.QuerySelectorAsync(Configs.Locators.PriceElement);
            string price = priceElement!=null?await priceElement.InnerTextAsync():"N/A";
            var locationElement = await yachtElement.QuerySelectorAsync(Configs.Locators.LocationElement);
            string location = locationElement!=null? await locationElement.InnerTextAsync():"N/A";
            string url = await yachtElement.GetAttributeAsync("href")??"N/A";

            yachts.Add(new YachtListing
            {
                Title = name,
                Price = price,
                Location = location,
                Url = $"{Configs.BaseUrl}{url}"
            });
        }
        return yachts;
    }

}
