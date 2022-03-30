using PriceTracker.Core.Entities;

namespace PriceTracker.Core.Services;

public interface IPriceChangeNotifier
{
    Task NotifySubscribers(IEnumerable<(PriceHistory current, PriceHistory previous)> priceChanges);
}