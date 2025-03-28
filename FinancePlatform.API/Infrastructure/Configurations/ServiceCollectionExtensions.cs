using FinancePlatform.API.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace FinancePlatform.API.Infrastructure.Configurations
{
    public static class ServiceCollectionExtensions
    {
         
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                using (var connection = new SqlConnection("Server=localhost,1433;Database=FinanceDB;User Id=sa;Password=YourStrong!Passw0rd;"))
                {
                    connection.Open();
                    Console.WriteLine("Conexão bem-sucedida!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            return services;
        }

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

        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration.GetValue<string>("RedisCacheSettings:RedisConnection");
            var instanceName = configuration.GetValue<string>("RedisCacheSettings:InstanceName");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;  
                options.InstanceName = instanceName;      
            });

            return services;
        }
    }
}
        