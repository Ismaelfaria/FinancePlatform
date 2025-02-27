using Mapster;

namespace FinancePlatform.API.Application.Mapper
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
