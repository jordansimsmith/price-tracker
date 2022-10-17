using PriceTracker.Core.Models;

namespace PriceTracker.Core.Interfaces;

public interface IPriceHistoryRetriever
{
    Task<IEnumerable<PriceHistoryModel>> RetrievePriceHistoryAsync();
}
