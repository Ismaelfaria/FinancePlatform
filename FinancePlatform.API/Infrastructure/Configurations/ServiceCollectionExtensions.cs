using FinancePlatform.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FinancePlatform.API.Infrastructure.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Recupera a string de conexão definida no appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FinanceDbContext>(options =>
                options.UseSqlServer(connectionString));
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

                // Opcional: incluir comentários XML para enriquecer a documentação
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
            return services;
        }
    }
}
        