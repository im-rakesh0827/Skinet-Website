using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;
public class ExceptionMiddleware(IHostEnvironment environment, RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, environment);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment environment)
    {
        try
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = environment.IsDevelopment()
                ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            return context.Response.WriteAsync(json);
        }
        catch (Exception)
        {
            throw;
        }
    }
}