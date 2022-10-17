using AngleSharp;
using AngleSharp.Dom;

namespace PriceTracker.Infrastructure.Scraping;

public abstract class BasePriceScraper
{
    public Guid UniqueId { get; }
    public string Name { get; }
    public string PageUrl { get; }

    protected BasePriceScraper(Guid uniqueId, string name, string pageUrl)
    {
        if (uniqueId == Guid.Empty)
        {
            throw new ArgumentException("UniqueId is empty");
        }
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(pageUrl);

        UniqueId = uniqueId;
        Name = name;
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
