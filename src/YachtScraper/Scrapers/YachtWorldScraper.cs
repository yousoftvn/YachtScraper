using Microsoft.Playwright;
using System;
using System.Linq;
using System.Text.Json;
using YachtScraper.Models;
using YachtScraper.Services;

namespace YachtScraper.Scrapers;
public partial class YachtWorldScraper:ScraperBase{
    public static class Configs {
        public static readonly string BaseUrl = "https://www.yachtworld.com/";
        public static readonly string SearchUrl = $"{BaseUrl}/yachtworld/search/boat?page=1&facets=countrySubdivision,make,condition,makeModel,type,class,country,countryRegion,countryCity,fuelType,hullMaterial,hullShape,minYear,maxYear,minMaxPercentilPrices,enginesConfiguration,enginesDriveType,numberOfEngines,minMaxPercentilPrices,minYear,maxYear,hullShape,enginesConfiguration,enginesDriveType,fuelType&fields=id,make,model,year,featureType,specifications.dimensions.lengths.nominal,location.address,aliases,owner.logos,owner.name,owner.rootName,owner.location.address.city,owner.location.address.country,price.hidden,price.type.amount,portalLink,class,media,isOemModel,isCurrentModel,attributes,previousPrice,mediaCount,cpybLogo&useMultiFacetedFacets=true&enableSponsoredSearch=true&locale=en-US&distance=200mi&pageSize=28&sort=recommended&multiFacetedBoatTypeClass=[%22power%22,[%22power-aft%22]]&wordsmithContentType=class&relatedBoatArticles=class&videoType=class&advantageSort=1&enableSponsoredSearchExactMatch=true&randomizedSponsoredBoatsSearch=true&randomSponsoredBoatsSize=8";
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
    private  async Task<List<YachtListing>> DoScrapingByPageContentAsync()
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
    private  async Task<List<YachtListing>> DoScrapingByAPIAsync()
    {
        var response = await Page.APIRequest.GetAsync(Configs.SearchUrl);

        if (response.Ok)
        {
            string jsonResponse = await response.TextAsync();
            var searchResults = JsonSerializer.Deserialize<YachtSearchResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return searchResults?.Search.Records?? default;
        }
        return default;
    }
    public override async Task<List<YachtListing>> DoScrapingAsync()
    {
        if(ScrapingMode==ScrapingMode.PageContent){
            return await DoScrapingByPageContentAsync();
        }
        return await DoScrapingByAPIAsync();
    }

}
