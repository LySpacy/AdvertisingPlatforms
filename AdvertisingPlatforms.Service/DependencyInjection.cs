using AdvertisingPlatforms.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AdvertisingPlatforms.Service;

public static  class DependencyInjection
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        
        // Singleton - для постоянной жизни в процессе работы приложения, т.к. данные храняться в памяти
        services.AddSingleton<IAdPlatformService, AdPlatformService>();
        
        services.AddScoped<IAdPlatformRepository, InMemoryAdPlatformRepository>();
        
        return services;
    }
}