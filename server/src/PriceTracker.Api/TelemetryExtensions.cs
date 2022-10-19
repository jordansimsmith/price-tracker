using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PriceTracker.Api;

public static class TelemetryExtensions
{
    public static void AddOpenTelemetryLogging(
        this ILoggingBuilder logging,
        IWebHostEnvironment environment,
        string endpoint
    )
    {
        logging.AddOpenTelemetry(options =>
        {
            options.ConfigureResource(r =>
            {
                r.AddService("price-tracker-server");
            });

            if (environment.IsDevelopment())
            {
                options.AddConsoleExporter();
            }
            else
            {
                options.AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(endpoint);
                    o.Protocol = OtlpExportProtocol.Grpc;
                });
            }
        });
    }

    public static void AddOpenTelemetryTracing(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        string endpoint
    )
    {
        services.AddOpenTelemetryTracing(options =>
        {
            options.ConfigureResource(r =>
            {
                r.AddService("price-tracker-server");
            });
            options.AddAspNetCoreInstrumentation();

            if (environment.IsDevelopment())
            {
                options.AddConsoleExporter();
            }
            else
            {
                options.AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(endpoint);
                    o.Protocol = OtlpExportProtocol.Grpc;
                });
            }
        });
    }
}
