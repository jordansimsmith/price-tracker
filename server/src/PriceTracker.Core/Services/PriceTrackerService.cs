using Microsoft.Extensions.Logging;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;

namespace PriceTracker.Core.Services;

public class PriceTrackerService : IPriceTrackerService
{
    private readonly ILogger<IPriceTrackerService> _logger;
    private readonly IPriceScraperFactory _priceScraperFactory;

    public PriceTrackerService(IPriceScraperFactory priceScraperFactory, ILogger<IPriceTrackerService> logger)
    {
        _priceScraperFactory = priceScraperFactory;
        _logger = logger;
    }

    public async Task TrackPricesAsync()
    {
        var scrapeResults = await ScrapePricesAsync();

        // TODO: compare against previous prices, save to db, dispatch email report
    }

    private async Task<IEnumerable<PriceScrapeResult>> ScrapePricesAsync()
    {
        var scrapers = _priceScraperFactory.CreatePriceScrapers();

        var priceResults = await Task.WhenAll(scrapers.Select(async s =>
        {
            try
            {
                var price = await s.ScrapePriceAsync();
                _logger.LogInformation("Retrieved price <{price}> for <{uniqueId}> <{name}> <{pageUrl}>", price,
                    s.UniqueId, s.Name, s.PageUrl);
                return new PriceScrapeResult(s.UniqueId, s.Name, s.PageUrl, price);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving price for <{uniqueId}> <{name}> <{pageUrl}>", s.UniqueId, s.Name,
                    s.PageUrl);
                return null;
            }
        }));

        var successfulPriceResults = priceResults
            .Where(result => result is not null)
            .Select(result => result!)
            .ToArray();

        return successfulPriceResults;
    }
}