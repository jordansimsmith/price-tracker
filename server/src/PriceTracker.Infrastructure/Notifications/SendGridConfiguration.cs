namespace PriceTracker.Infrastructure.Notifications;

public class SendGridConfiguration
{
    public string SenderAddress { get; set; }
    public string SenderName { get; set; }
    public string PriceChangeTemplateId { get; set; }
    public string ApiKey { get; set; }
}
