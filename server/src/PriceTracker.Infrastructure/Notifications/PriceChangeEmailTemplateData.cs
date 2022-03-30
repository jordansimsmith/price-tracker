using Newtonsoft.Json;

namespace PriceTracker.Infrastructure.Notifications;

public class PriceChangeEmailTemplateData
{
    [JsonProperty("name")] public string Name { get; set; }
    
    [JsonProperty("price_changes")] public IEnumerable<PriceChangeTemplateModel> PriceChanges { get; set; }
}

public class PriceChangeTemplateModel
{
    [JsonProperty("target_name")] public string TargetName { get; set; }

    [JsonProperty("target_page_url")] public string TargetPageUrl { get; set; }

    [JsonProperty("previous_price")] public decimal PreviousPrice { get; set; }

    [JsonProperty("current_price")] public decimal CurrentPrice { get; set; }
}