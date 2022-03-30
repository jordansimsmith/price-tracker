using Microsoft.Extensions.Logging;
using PriceTracker.Core.Entities;
using PriceTracker.Core.Services;

namespace PriceTracker.Infrastructure.Notifications;

public class EmailPriceChangeNotifier : IPriceChangeNotifier
{
    private readonly ILogger<EmailPriceChangeNotifier> _logger;

    public EmailPriceChangeNotifier(ILogger<EmailPriceChangeNotifier> logger)
    {
        _logger = logger;
    }

    public Task NotifySubscribers(IEnumerable<(PriceHistory current, PriceHistory previous)> priceChanges)
    {
        foreach (var (current, previous) in priceChanges)
        {
            _logger.LogInformation("Price change <{oldPrice}> to <{newPrice}> for <{uniqueId}> <{name}> <{pageUrl}>",
                previous.Price, current.Price, current.TargetUniqueId, current.TargetName, current.TargetPageUrl);
        }

        return Task.CompletedTask;
    }
}