using PriceTracker.Core.Models;

namespace PriceTracker.Core.Interfaces;

public interface IPriceScraper
{
    public Guid UniqueId { get; }
    public string Name { get; }
    public string PageUrl { get; }

    Task<PriceScrapeResult> ScrapePriceAsync();
}
