using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Configurations
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSingleton(Log.Logger);
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();  // Clear default providers
                loggingBuilder.AddSerilog();  // Add Serilog provider
            });

            return services;
        }
    }
}
