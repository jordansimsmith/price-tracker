using PriceTracker.Core.Models;

namespace PriceTracker.Core.Interfaces;

public interface IPriceScraperService
{
    Task<IEnumerable<PriceScrapeResult>> ScrapePricesAsync();
}