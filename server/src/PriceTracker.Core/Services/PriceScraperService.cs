using Microsoft.Extensions.Logging;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;

namespace PriceTracker.Core.Services;

public class PriceScraperService : IPriceScraperService
{
    private readonly ILogger<IPriceScraperService> _logger;
    private readonly IPriceScraperFactory _priceScraperFactory;

    public PriceScraperService(IPriceScraperFactory priceScraperFactory, ILogger<IPriceScraperService> logger)
    {
        _priceScraperFactory = priceScraperFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<PriceScrapeResult>> ScrapePricesAsync()
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
            .Select(result => result!);

        return successfulPriceResults;
    }
}