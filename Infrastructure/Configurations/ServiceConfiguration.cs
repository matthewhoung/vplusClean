using Application.Interfaces.Security;
using Application.Interfaces.Tasks;
using Application.Interfaces.Users;
using Application.Services.Tasks;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Tasks;
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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITaskService,TaskService>();

            return services;
        }
    }
}
