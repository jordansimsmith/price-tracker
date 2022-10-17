namespace PriceTracker.Core.Models;

public class TrackingTargetModel
{
    public string PageUrl { get; set; }
    public Guid UniqueId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}
