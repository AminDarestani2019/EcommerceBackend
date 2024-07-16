//#define PrivateConnection
#define LocalConnection
//#define PublicConnection

using Application.Contracts;
using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class ConfigureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {

#if PublicConnection
                option.UseSqlServer(configuration.GetConnectionString("PublicConnection"));
#elif PrivateConnection
                option.UseSqlServer(configuration.GetConnectionString("PrivateConnection"));
#elif LocalConnection
                option.UseSqlServer(configuration.GetConnectionString("LocalConnection"));
#endif

            });
            //connection string redis
            services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
#if PublicConnection
                var options = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisPublic") ?? string.Empty, true);
                return ConnectionMultiplexer.Connect(options);
#elif PrivateConnection
                var options = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisPrivate") ?? string.Empty, true);
                return ConnectionMultiplexer.Connect(options);
#elif LocalConnection
                var options = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisLocal") ?? string.Empty, true);
                return ConnectionMultiplexer.Connect(options);
#endif
            });
         
            //Identity
            services.AddIdentityService(configuration);
            services.AddScoped<ITokenService, TokenService>();
            //DI
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
