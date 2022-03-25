using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PriceTracker.Core.Entities;

namespace PriceTracker.Infrastructure.Data;

public class PriceTrackerContext : DbContext
{
    public PriceTrackerContext(DbContextOptions<PriceTrackerContext> options) : base(options)
    {
    }

    public DbSet<PriceHistory> PriceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}