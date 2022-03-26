using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using PriceTracker.Core;
using PriceTracker.Core.Interfaces;
using PriceTracker.Core.Models;
using PriceTracker.Infrastructure;
using PriceTracker.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPriceTrackerContext(builder.Configuration.GetConnectionString("PriceTracker"));
builder.Services.AddInfrastructureServices();
builder.Services.AddCoreServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TrackingTargetConfiguration>(options =>
{
    options.TrackingTargets = builder.Configuration
        .GetSection("TrackingTargets")
        .GetChildren()
        .Select(c => c.Get<TrackingTargetModel>())
        .ToArray();
});

builder.Services.Configure<SubscriberConfiguration>(options =>
{
    options.Subscribers = builder.Configuration
        .GetSection("Subscribers")
        .GetChildren()
        .Select(c => c.Get<SubscriberModel>())
        .ToArray();
});

builder.Services.AddHangfireContext(builder.Configuration.GetConnectionString("PriceTrackerHangfire"));
builder.Services.AddHangfire(configuration =>
{
    configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("PriceTrackerHangfire"));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var priceTrackerContext = scope.ServiceProvider.GetService<PriceTrackerContext>();
await priceTrackerContext!.Database.MigrateAsync();

await using var hangfireContext = scope.ServiceProvider.GetService<HangfireContext>();
await hangfireContext!.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    IsReadOnlyFunc = _ => !app.Environment.IsDevelopment()
});

app.MapControllers();

RecurringJob.AddOrUpdate<IPriceTrackerService>(x => x.TrackPricesAsync(), Cron.Daily);

app.Run();