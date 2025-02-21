using Microsoft.Playwright;
using YachtScraper.Scrapers;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions {
                    Headless = false
                }
              );
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            var scraper = new YachtWorldScraper(page);
            await scraper.GoToAsync();
           

        }
    }
}
