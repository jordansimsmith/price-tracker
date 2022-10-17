namespace PriceTracker.Core.Models;

public record PriceScrapeResult(
    Guid UniqueId,
    string Name,
    string PageUrl,
    decimal Price,
    bool InStock
);
