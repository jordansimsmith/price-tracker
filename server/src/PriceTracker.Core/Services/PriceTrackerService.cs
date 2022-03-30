using Microsoft.Extensions.Logging;
using PriceTracker.Core.Entities;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;

namespace PriceTracker.Core.Services;

public class PriceTrackerService : IPriceTrackerService
{
    private readonly ILogger<IPriceTrackerService> _logger;
    private readonly IPriceScraperFactory _priceScraperFactory;
    private readonly IPriceHistoryRepository _priceHistoryRepository;
    private readonly IPriceChangeNotifier _priceChangeNotifier;

    public PriceTrackerService(
        IPriceScraperFactory priceScraperFactory,
        ILogger<IPriceTrackerService> logger,
        IPriceHistoryRepository priceHistoryRepository,
        IPriceChangeNotifier priceChangeNotifier
    )
    {
        _priceScraperFactory = priceScraperFactory;
        _logger = logger;
        _priceHistoryRepository = priceHistoryRepository;
        _priceChangeNotifier = priceChangeNotifier;
    }

    public async Task TrackPricesAsync()
    {
        var scrapeResults = await ScrapePricesAsync();

        var priceChanges = new List<(PriceHistory current, PriceHistory previous)>();

        foreach (var result in scrapeResults)
        {
            var current = new PriceHistory
            {
                Date = DateTime.UtcNow,
                Price = result.Price,
                TargetName = result.Name,
                TargetPageUrl = result.PageUrl,
                TargetUniqueId = result.UniqueId,
            };

            var previous = await _priceHistoryRepository.FindLatestOrDefault(result.UniqueId);

            await _priceHistoryRepository.AddAsync(current);

            if (previous != null && previous.Price != current.Price)
            {
                priceChanges.Add((current, previous));
            }
        }

        if (priceChanges.Any())
        {
            await _priceChangeNotifier.NotifySubscribersAsync(priceChanges);
        }
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