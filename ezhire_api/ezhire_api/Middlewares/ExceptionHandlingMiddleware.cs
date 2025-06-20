using System.Net;
using System.Text.Json;
using ezhire_api.Exceptions;

namespace ezhire_api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = ex switch
        {
            BadRequest => (HttpStatusCode.BadRequest, ex.Message),
            NotFound => (HttpStatusCode.NotFound, ex.Message),
            UnprocessableEntity => (HttpStatusCode.UnprocessableEntity, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        if (statusCode == HttpStatusCode.InternalServerError) Console.Error.WriteLine(ex);

        return context.Response.WriteAsync(result);
    }
}