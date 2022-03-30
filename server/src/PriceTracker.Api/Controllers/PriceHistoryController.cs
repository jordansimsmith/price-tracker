using Microsoft.AspNetCore.Mvc;
using PriceTracker.Core.Interfaces;

namespace PriceTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceHistoryController: ControllerBase
{
    private readonly IPriceHistoryRetriever _priceHistoryRetriever;

    public PriceHistoryController(IPriceHistoryRetriever priceHistoryRetriever)
    {
        _priceHistoryRetriever = priceHistoryRetriever;
    }

    [HttpGet]
    public async Task<ActionResult> GetPriceHistoryAsync()
    {
        var history =  await _priceHistoryRetriever.RetrievePriceHistoryAsync();

        return Ok(history);
    }
}