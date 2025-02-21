namespace YachtScraper.Models;

public class SearchResult
{
    public int Count { get; set; }
    public List<YachtListing> Records { get; set; }
}
