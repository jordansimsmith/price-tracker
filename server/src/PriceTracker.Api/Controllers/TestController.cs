using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;
using PriceTracker.Infrastructure.Scraping;

namespace PriceTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IPriceScraperFactory _priceScraperFactory;

    public TestController(ILogger<TestController> logger, IPriceScraperFactory priceScraperFactory)
    {
        _logger = logger;
        _priceScraperFactory = priceScraperFactory;
    }

    [HttpGet("/test")]
    public async Task<ActionResult> TestAsync()
    {
        var scrapers = _priceScraperFactory.CreatePriceScrapers();

        var prices = await Task.WhenAll(scrapers.Select(s => s.ScrapePriceAsync()));
       
        return Ok(prices);
    }
}