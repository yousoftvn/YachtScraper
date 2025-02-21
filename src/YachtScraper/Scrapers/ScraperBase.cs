using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YachtScraper.Scrapers;

public abstract class ScraperBase {
   
    protected IPage Page { get; private set; }
    protected string BaseUrl { get; }
    protected ScrapingMode ScrapingMode { get; }
    public ScraperBase(IPage page, string baseUrl,ScrapingMode scrapingMode=ScrapingMode.PageContent) {
        Page = page;
        BaseUrl = baseUrl;
        ScrapingMode = scrapingMode;
    }
    public async Task GoToAsync() {
       await Page.GotoAsync(BaseUrl);
    }
    

    public virtual async Task<List<Models.YachtListing>> DoScrapingAsync() {
        throw new NotImplementedException();
    }
    
}
