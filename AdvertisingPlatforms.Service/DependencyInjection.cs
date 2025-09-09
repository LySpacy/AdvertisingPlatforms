using AdvertisingPlatforms.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AdvertisingPlatforms.Service;

public static  class DependencyInjection
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        
        // Singleton - для постоянной жизни в процессе работы приложения, т.к. данные храняться в памяти
        services.AddSingleton<IAdPlatformRepository, InMemoryAdPlatformRepository>();
        services.AddSingleton<IAdPlatformService, AdPlatformService>();
        
        return services;
    }
}