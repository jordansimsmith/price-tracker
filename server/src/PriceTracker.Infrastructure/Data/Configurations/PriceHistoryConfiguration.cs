using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Core.Entities;

namespace PriceTracker.Infrastructure.Data.Configurations;

public class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Date).IsRequired();
        builder.Property(o => o.Price).IsRequired();
        builder.Property(o => o.TargetUniqueId).IsRequired();
        builder.Property(o => o.TargetName).IsRequired();
        builder.Property(o => o.TargetPageUrl).IsRequired();
    }
}