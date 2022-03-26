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

    private readonly IPriceScraperService _priceScraperService;

    public TestController(IPriceScraperService priceScraperService)
    {
        _priceScraperService = priceScraperService;
    }

    [HttpGet("/test")]
    public async Task<ActionResult> TestAsync()
    {
        var results = await _priceScraperService.ScrapePricesAsync();
       
        return Ok(results);
    }
}