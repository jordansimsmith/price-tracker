using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;

namespace PriceTracker.Infrastructure.Scraping;

public class ChemistWarehousePriceScraper : BasePriceScraper, IPriceScraper
{
    public ChemistWarehousePriceScraper(Guid uniqueId, string name, string pageUrl)
        : base(uniqueId, name, pageUrl) { }

    public async Task<PriceScrapeResult> ScrapePriceAsync()
    {
        var document = await LoadDocumentAsync();

        var priceString = document
            .QuerySelector(".product_details")
            ?.QuerySelector(".Price")
            ?.QuerySelector(".product__price")
            ?.TextContent;

        if (priceString is null)
        {
            throw new SystemException("Could not find the price in the document");
        }

        var priceWithoutCurrency = priceString.Replace("$", "");
        if (!decimal.TryParse(priceWithoutCurrency, out var price))
        {
            throw new SystemException("Could not extract the price from the element");
        }

        var inStoreOnly =
            document.QuerySelector("#product_images")?.QuerySelector("img[alt^=\"In store only\"]")
                is not null;

        var inStock = !inStoreOnly;

        return new PriceScrapeResult(UniqueId, Name, PageUrl, price, inStock);
    }
}
