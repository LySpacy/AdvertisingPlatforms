using Microsoft.OpenApi.Models;

namespace AdvertisingPlatforms.API;
internal static class Swagger
{
    public static string AppName { get; set; } = typeof(Swagger).Assembly.GetName().Name ?? "";

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Подключение XML-документации
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories);
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile, true);
            }

            // Общие настройки Swagger
            options.SupportNonNullableReferenceTypes();
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
            options.SwaggerDoc("v1", new OpenApiInfo { Title = AppName, Version = "v1" });
        });

        return services;
    }

    public static void UseCustomSwagger(this WebApplication app, string pathBase = "")
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.{json|yaml}";
        });

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", $"{AppName} v1");
        });
    }
}