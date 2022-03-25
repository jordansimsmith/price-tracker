using AngleSharp;
using AngleSharp.Dom;

namespace PriceTracker.Infrastructure.Scraping;

public abstract class BasePriceScraper
{
    protected readonly string PageUrl;

    protected BasePriceScraper(string pageUrl)
    {
        ArgumentNullException.ThrowIfNull(pageUrl);
        PageUrl = pageUrl;
    }

    protected async Task<IDocument> LoadDocumentAsync()
    {
        var configuration = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(configuration);
        var document = await context.OpenAsync(PageUrl);

        if (document is null)
        {
            throw new SystemException($"Could not load the document for {PageUrl}");
        }
        
        return document;
    }
}