using Microsoft.Playwright;
using YachtScraper.Scrapers;
using YachtScraper.Services;
using System.Text.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var yachtListings = await new ScrapingService().RunAsync();
            if(yachtListings is null || yachtListings.Count == 0){
                Console.WriteLine("No data found!");
                return;
            }
            var serializerOption = new JsonSerializerOptions { WriteIndented = true };
            
            foreach (var yacht in yachtListings){
                var json = JsonSerializer.Serialize(yacht,serializerOption);
                Console.WriteLine(json);
            }
        }
    }
}
