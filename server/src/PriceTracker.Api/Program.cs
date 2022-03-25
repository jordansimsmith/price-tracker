using Microsoft.EntityFrameworkCore;
using PriceTracker.Core.Models;
using PriceTracker.Infrastructure;
using PriceTracker.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPriceTrackerContext(builder.Configuration.GetConnectionString("PriceTracker"));
builder.Services.AddInfrastructureServices();

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

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var context = scope.ServiceProvider.GetService<PriceTrackerContext>();
await context!.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();