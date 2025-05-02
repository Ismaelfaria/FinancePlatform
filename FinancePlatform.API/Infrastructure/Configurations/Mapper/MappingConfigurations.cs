using Mapster;

namespace FinancePlatform.API.Infrastructure.Configurations.Mapper
{
    public static class MappingConfigurations
    {
        public static IServiceCollection RegisterMaps(this IServiceCollection services)
        {
            services.AddMapster();
            return services;
        }
    }
}
