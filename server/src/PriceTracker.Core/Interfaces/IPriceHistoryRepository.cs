using PriceTracker.Core.Entities;

namespace PriceTracker.Core.Interfaces;

public interface IPriceHistoryRepository
{
    Task<PriceHistory> AddAsync(PriceHistory priceHistory);

    Task<IEnumerable<PriceHistory>> GetHistoryAsync();
}