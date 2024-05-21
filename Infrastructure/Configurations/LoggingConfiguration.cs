using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Configurations
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File(
                    path: "/logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    formatter: new Serilog.Formatting.Compact.CompactJsonFormatter())
                .CreateLogger();

            services.AddSingleton(Log.Logger);
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            return services;
        }
    }
}
