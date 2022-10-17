namespace PriceTracker.Core.Models;

public class PriceHistoryModel
{
    public DateTime Date { get; set; }
    public decimal Price { get; set; }

    public Guid TargetUniqueId { get; set; }
    public string TargetName { get; set; }
    public string TargetPageUrl { get; set; }
}
