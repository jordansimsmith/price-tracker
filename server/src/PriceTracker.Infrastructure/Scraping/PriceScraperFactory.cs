using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PriceTracker.Core;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;

namespace PriceTracker.Infrastructure.Scraping;

public class PriceScraperFactory: IPriceScraperFactory
{
    private readonly IEnumerable<TrackingTargetModel> _trackingTargets;
    private readonly ILogger<PriceScraperFactory> _logger;

    public PriceScraperFactory(IOptions<TrackingTargetConfiguration> trackingTargetsOptions,
        ILogger<PriceScraperFactory> logger)
    {
        _logger = logger;
        _trackingTargets = trackingTargetsOptions.Value.TrackingTargets;
    }

    public IEnumerable<IPriceScraper> CreatePriceScrapers()
    {
        return _trackingTargets.Select(target =>
            {
                switch (target.Type)
                {
                    case nameof(ChemistWarehousePriceScraper):
                        return new ChemistWarehousePriceScraper(target.UniqueId, target.Name, target.PageUrl) as IPriceScraper;
                    default:
                        _logger.LogWarning(
                            "No scraping implementation found for tracking target type <{type}>", target.Type);
                        return null;
                }
            })
            .Where(target => target is not null)
            .Select(target => target!);
    }
}