using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Services;

namespace PriceTracker.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IPriceTrackerService, PriceTrackerService>();
        services.AddScoped<IPriceHistoryRetriever, PriceHistoryRetriever>();

        return services;
    }
}