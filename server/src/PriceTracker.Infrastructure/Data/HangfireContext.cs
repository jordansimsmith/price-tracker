using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Infrastructure.Data;

public class HangfireContext : DbContext
{
    public HangfireContext(DbContextOptions<HangfireContext> options) : base(options) { }
}
