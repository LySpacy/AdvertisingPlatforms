using AdvertisingPlatforms.Service;

namespace AdvertisingPlatforms.API;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Добавляем контроллеры
        builder.Services.AddControllers();

        // Подключаем Swagger
        builder.Services.AddCustomSwagger();

        // Регистрируем слои приложения (сервисы + репозитории)
        builder.Services.RegisterApplicationLayer();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Подключаем Swagger UI
        app.UseCustomSwagger();

        app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        // Маршрутизация контроллеров
        app.MapControllers();

        return app;
    }
}