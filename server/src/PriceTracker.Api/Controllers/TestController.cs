using Microsoft.AspNetCore.Mvc;
using PriceTracker.Infrastructure.Scraping;

namespace PriceTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/test")]
    public async Task<decimal> TestAsync()
    {

        const string pageUrl = @"https://www.chemistwarehouse.co.nz/buy/98676/inc-100-dynamic-whey-cookies-and-cream-flavour-2kg";
        var chemistWarehousePriceScraper = new ChemistWarehousePriceScraper(pageUrl);
        var price = await chemistWarehousePriceScraper.ScrapePriceAsync();

        return price;
    }
}