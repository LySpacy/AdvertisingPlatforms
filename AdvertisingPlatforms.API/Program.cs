using AdvertisingPlatforms.API;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.ConfigureServices();
builder.Services.AddControllers();

var app = builder.Build();

// Конфигурация пайплайна
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Подключаем Swagger UI
app.ConfigurePipeline();

// Маршруты контроллеров
app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});

// Запуск приложения
app.Run();