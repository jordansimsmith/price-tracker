namespace PriceTracker.Core;

public interface IPriceScraper
{
    Task<decimal> ScrapePriceAsync();
}