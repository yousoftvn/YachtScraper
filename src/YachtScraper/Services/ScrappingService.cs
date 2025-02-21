using Microsoft.Playwright;
using YachtScraper.Models;
using YachtScraper.Scrapers;

namespace YachtScraper.Services;
public class ScrapingService{
    
    public static async Task RandomWait(){
        var random = new Random();
        int delay = random.Next(Configs.RandomWaitFrom, Configs.RandomWaitTo);
        await Task.Delay(delay); 
    }

    public async Task<List<YachtListing>> RunAsync(){
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions {
                    Headless = Configs.Headless
                }
            );
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var scraper = new YachtWorldScraper(page,ScrapingMode.ApiResponse);
        await scraper.GoToAsync();
        await ScrapingService.RandomWait();
        return await scraper.DoScrapingAsync();
    }
}