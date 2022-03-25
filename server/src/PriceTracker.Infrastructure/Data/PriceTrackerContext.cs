using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Infrastructure.Data;

public class PriceTrackerContext : DbContext
{
    public PriceTrackerContext(DbContextOptions<PriceTrackerContext> options) : base(options)
    {
    }
}