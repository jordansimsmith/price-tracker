namespace PriceTracker.Core.Entities;

public class PriceHistory
{
    public int? Id { get; set; }
    
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
        
    public Guid TargetUniqueId { get; set; }
    public string TargetName { get; set; }
    public string TargetPageUrl { get; set; }
}