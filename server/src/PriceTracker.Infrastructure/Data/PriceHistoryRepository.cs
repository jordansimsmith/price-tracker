using Microsoft.EntityFrameworkCore;
using PriceTracker.Core.Entities;
using PriceTracker.Core.Interfaces;

namespace PriceTracker.Infrastructure.Data;

public class PriceHistoryRepository : IPriceHistoryRepository
{
    private readonly PriceTrackerContext _context;

    public PriceHistoryRepository(PriceTrackerContext context)
    {
        _context = context;
    }

    public async Task<PriceHistory> AddAsync(PriceHistory priceHistory)
    {
        var savedPriceHistory = await _context.AddAsync(priceHistory);
        await _context.SaveChangesAsync();

        return savedPriceHistory.Entity;
    }

    public async Task<IEnumerable<PriceHistory>> GetHistoryAsync()
    {
        return await _context.PriceHistories.OrderByDescending(p => p.Date).ToListAsync();
    }

    public Task<PriceHistory?> FindLatestOrDefault(Guid uniqueId)
    {
        return _context.PriceHistories
            .Where(p => p.TargetUniqueId == uniqueId)
            .OrderByDescending(p => p.Date)
            .FirstOrDefaultAsync();
    }
}
