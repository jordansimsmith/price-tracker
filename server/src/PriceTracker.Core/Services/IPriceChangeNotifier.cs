using PriceTracker.Core.Entities;

namespace PriceTracker.Core.Services;

public interface IPriceChangeNotifier
{
    Task NotifySubscribersAsync(
        IEnumerable<(PriceHistory current, PriceHistory previous)> priceChanges
    );
}
