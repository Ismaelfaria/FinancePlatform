
using FinancePlatform.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FinancePlatform.API.Infrastructure.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FinancePlatform API",
                    Version = "v1",
                    Description = "Aplicação de gestão financeira",
                    Contact = new OpenApiContact
                    {
                        Name = "Ismael Lima",
                        Email = "limaismael4444@gmail.com"
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
            return services;
        }

        public static IServiceCollection AddMySqlConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MySQL");

            services.AddDbContext<FinanceDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            return services;
        }

        public static IServiceCollection AddRedisCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString("RedisConnection");

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "RedisInstance";
                options.Configuration = redisConnection;
            });

            return services;
        }
    }
}
        