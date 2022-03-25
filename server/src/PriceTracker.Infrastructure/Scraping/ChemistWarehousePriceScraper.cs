using System.Transactions;
using AngleSharp;
using PriceTracker.Core;

namespace PriceTracker.Infrastructure.Scraping;

public class ChemistWarehousePriceScraper : BasePriceScraper, IPriceScraper
{
    public ChemistWarehousePriceScraper(string pageUrl) : base(pageUrl)
    {
    }

    public async Task<decimal> ScrapePriceAsync()
    {
        var document = await LoadDocumentAsync();

        var priceString = document
            .QuerySelector(".product_details")?
            .QuerySelector(".Price")?
            .QuerySelector(".product__price")?
            .TextContent;

        if (priceString is null)
        {
            throw new SystemException("Could not find the price in the document");
        }

        var priceWithoutCurrency = priceString.Replace("$", "");
        if (!decimal.TryParse(priceWithoutCurrency, out var price))
        {
            throw new SystemException("Could not extract the price from the element");
        }
        
        return price;
    }
}