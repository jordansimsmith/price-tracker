using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PriceTracker.Core.Entities;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;
using PriceTracker.Infrastructure.Scraping;

namespace PriceTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IPriceScraperService _priceScraperService;
    private readonly IPriceHistoryRepository _priceHistoryRepository;

    public TestController(IPriceScraperService priceScraperService, IPriceHistoryRepository priceHistoryRepository)
    {
        _priceScraperService = priceScraperService;
        _priceHistoryRepository = priceHistoryRepository;
    }

    [HttpGet("/test")]
    public async Task<ActionResult> TestAsync()
    {
        var results = await _priceScraperService.ScrapePricesAsync();
        
        foreach (var priceScrapeResult in results)
        {
            var priceHistory = new PriceHistory
            {
                Date = DateTime.UtcNow,
                Price = priceScrapeResult.Price,
                TargetName = priceScrapeResult.Name,
                TargetPageUrl = priceScrapeResult.PageUrl,
                TargetUniqueId = priceScrapeResult.UniqueId
            };

            await _priceHistoryRepository.AddAsync(priceHistory);
        }

        var history = await _priceHistoryRepository.GetHistoryAsync();
       
        return Ok(history);
    }
}