using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;



namespace Infrastructure.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(provider =>
                new MySqlConnection(configuration.GetConnectionString("DefaultConnection")));

            //services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            //services.AddScoped<IJwtService, JwtService>();
            //services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
