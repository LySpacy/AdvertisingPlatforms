using System.Net;
using System.Text.Json;
using AdvertisingPlatforms.Service.Common.Exceptions;

namespace AdvertisingPlatforms.API;

/// <summary>
/// Глобальный обработчик ошибок для ASP.NET Core.
/// Перехватывает исключения, возникающие в процессе обработки запроса,
/// и возвращает клиенту корректный HTTP-ответ с описанием ошибки.
/// </summary>
public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Конструктор middleware.
    /// </summary>
    /// <param name="next">Следующий компонент в конвейере обработки HTTP-запросов.</param>
    public GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Основной метод middleware, который обрабатывает входящие HTTP-запросы.
    /// Перехватывает исключения и формирует соответствующий HTTP-ответ.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>Асинхронная задача.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidAdPlatformFileException ex)
        {
            await HandleExceptionAsync(HttpStatusCode.BadRequest, context, ex);
        }
        catch (InvalidDataException ex)
        {
            await HandleExceptionAsync(HttpStatusCode.BadRequest, context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(HttpStatusCode.InternalServerError, context, ex);
        }
    }

    /// <summary>
    /// Формирует HTTP-ответ с заданным статусом и сериализованным сообщением об ошибке.
    /// </summary>
    /// <param name="statusCode">HTTP-статус, который будет отправлен клиенту.</param>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="exception">Исключение, которое необходимо обработать.</param>
    /// <returns>Асинхронная задача.</returns>
    private static async Task HandleExceptionAsync(HttpStatusCode statusCode, HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            Message = exception.Message
        });

        await context.Response.WriteAsync(result);
    }
}