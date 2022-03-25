using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PriceTracker.Core.Models;
using PriceTracker.Infrastructure.Scraping;

namespace PriceTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IEnumerable<TrackingTargetModel> _trackingTargetConfigurations;

    public TestController(ILogger<TestController> logger, IOptions<TrackingTargetConfiguration> trackingTargetConfigurationOptions)
    {
        _logger = logger;
        _trackingTargetConfigurations = trackingTargetConfigurationOptions.Value.TrackingTargets;
    }

    [HttpGet("/test")]
    public async Task<ActionResult> TestAsync()
    {
        const string pageUrl = @"https://www.chemistwarehouse.co.nz/buy/98676/inc-100-dynamic-whey-cookies-and-cream-flavour-2kg";
        var chemistWarehousePriceScraper = new ChemistWarehousePriceScraper(pageUrl);
        var price = await chemistWarehousePriceScraper.ScrapePriceAsync();
        
        return Ok(_trackingTargetConfigurations);
    }
}