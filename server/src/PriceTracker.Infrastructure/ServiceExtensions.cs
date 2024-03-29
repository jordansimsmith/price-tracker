using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Services;
using PriceTracker.Infrastructure.Data;
using PriceTracker.Infrastructure.Notifications;
using PriceTracker.Infrastructure.Scraping;

namespace PriceTracker.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddPriceTrackerContext(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContext<PriceTrackerContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddHangfireContext(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContext<HangfireContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
        services.AddScoped<IPriceScraperFactory, PriceScraperFactory>();
        services.AddScoped<IPriceChangeNotifier, EmailPriceChangeNotifier>();

        return services;
    }
}
