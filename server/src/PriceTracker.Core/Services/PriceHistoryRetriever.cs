using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;

namespace PriceTracker.Core.Services;

public class PriceHistoryRetriever : IPriceHistoryRetriever
{
    private readonly IPriceHistoryRepository _priceHistoryRepository;

    public PriceHistoryRetriever(IPriceHistoryRepository priceHistoryRepository)
    {
        _priceHistoryRepository = priceHistoryRepository;
    }

    public async Task<IEnumerable<PriceHistoryModel>> RetrievePriceHistoryAsync()
    {
        var priceHistories = await _priceHistoryRepository.GetHistoryAsync();

        return priceHistories.Select(
            p =>
                new PriceHistoryModel
                {
                    Date = p.Date,
                    Price = p.Price,
                    TargetName = p.TargetName,
                    TargetPageUrl = p.TargetPageUrl,
                    TargetUniqueId = p.TargetUniqueId
                }
        );
    }
}
