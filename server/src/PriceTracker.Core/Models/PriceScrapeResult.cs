namespace PriceTracker.Core.Models;

public record PriceScrapeResult(Guid UniqueId, string Name, string PageUrl, decimal Price)
{
    public Guid UniqueId { get; set; } = UniqueId;
    public string Name { get; set; } = Name;
    public string PageUrl { get; set; } = PageUrl;
    public decimal Price { get; set; } = Price;
}
