using System.Net;
using System.Text.Json;
using AdvertisingPlatforms.Service.Common.Exceptions;

namespace AdvertisingPlatforms.API;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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
