using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Infrastructure.Data;

namespace PriceTracker.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddPriceTrackerContext(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<PriceTrackerContext>(options => options.UseNpgsql(connectionString));
    }
}