namespace PriceTracker.Core.Interfaces;

public interface IPriceScraperFactory
{
    IEnumerable<IPriceScraper> CreatePriceScrapers();
}